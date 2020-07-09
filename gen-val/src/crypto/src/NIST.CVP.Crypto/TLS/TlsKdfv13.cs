using System;
using System.Text;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.Common.KDF.Components.TLS;
using NIST.CVP.Crypto.Common.KDF.HKDF;
using NIST.CVP.Math;
using NIST.CVP.Math.Helpers;

namespace NIST.CVP.Crypto.TLS
{
	/// <summary>
	/// TLS v1.3 KDF from https://tools.ietf.org/html/rfc8446#section-7.1
	/// </summary>
	public class TlsKdfv13 : ITlsKdf_v1_3
	{
		private readonly IHkdf _hkdf;
		private readonly ISha _sha;
		private readonly int _hashOutputLengthBits;
		private readonly BitString _bitstringHashLengthBits;
		
		public TlsKdfv13(IHkdf hkdf, ISha sha, int hashOutputLengthBits)
		{
			_hkdf = hkdf;
			_sha = sha;
			_hashOutputLengthBits = hashOutputLengthBits;
			_bitstringHashLengthBits = new BitString(_hashOutputLengthBits);
		}

		public TlsKdfV13EarlySecretResult GetDerivedEarlySecret(bool isExternalPsk, BitString preSharedKey, BitString clientHello)
		{
			var earlySecret = _hkdf.Extract(_bitstringHashLengthBits, preSharedKey);
			
			var binderKey = DeriveSecret(earlySecret, isExternalPsk ? "ext binder" : "res binder", BitString.Empty());
			var clientEarlyTrafficSecret = DeriveSecret(earlySecret, "c e traffic", clientHello);
			var earlyExporterMasterSecret = DeriveSecret(earlySecret, "e exp master", clientHello);

			var derivedEarlySecret = DeriveSecret(earlySecret, "derived", BitString.Empty());
			
			return new TlsKdfV13EarlySecretResult()
			{
				EarlySecret = earlySecret,
				BinderKey = binderKey,
				ClientEarlyTrafficSecret = clientEarlyTrafficSecret,
				EarlyExporterMasterSecret = earlyExporterMasterSecret,
				DerivedEarlySecret = derivedEarlySecret,
			};
		}

		public TlsKdfV13HandshakeSecretResult GetDerivedHandshakeSecret(BitString diffieHellmanSharedSecret, BitString salt, 
			BitString clientHello, BitString serverHello)
		{
			var handshakeSecret = _hkdf.Extract(salt, diffieHellmanSharedSecret);
			
			var clientHandshakeTrafficSecret = DeriveSecret(handshakeSecret, "c hs traffic", clientHello, serverHello);
			var serverHandshakeTrafficSecret = DeriveSecret(handshakeSecret, "s hs traffic", clientHello, serverHello);

			var derivedHandshakeSecret = DeriveSecret(handshakeSecret, "derived", BitString.Empty());
			
			return new TlsKdfV13HandshakeSecretResult()
			{
				HandshakeSecret = handshakeSecret,
				ClientHandshakeTrafficSecret = clientHandshakeTrafficSecret,
				ServerHandshakeTrafficSecret = serverHandshakeTrafficSecret,
				DerivedHandshakeSecret = derivedHandshakeSecret,
			};
		}

		public TlsKdfV13MasterSecretResult GetDerivedMasterSecret(BitString salt, 
			BitString clientHello, BitString clientFinished,
			BitString serverFinished)
		{
			var masterSecret = _hkdf.Extract(salt, _bitstringHashLengthBits);

			var clientApplicationTrafficSecret = DeriveSecret(masterSecret, "c ap traffic", clientHello, serverFinished);
			var serverApplicationTrafficSecret = DeriveSecret(masterSecret, "s ap traffic", clientHello, serverFinished);
			var exporterMasterSecret = DeriveSecret(masterSecret, "exp master", clientHello, serverFinished);
			var resumptionMasterSecret = DeriveSecret(masterSecret, "res master", clientHello, clientFinished);

			return new TlsKdfV13MasterSecretResult()
			{
				MasterSecret = masterSecret,
				ClientApplicationTrafficSecret = clientApplicationTrafficSecret,
				ServerApplicationTrafficSecret = serverApplicationTrafficSecret,
				ExporterMasterSecret = exporterMasterSecret,
				ResumptionMasterSecret = resumptionMasterSecret
			};
		}

		public TlsKdfV13FullResult GetFullKdf(bool isExternalPsk, BitString preSharedKey, BitString diffieHellmanSharedSecret,
			BitString clientHello, BitString clientFinished, BitString serverHello, BitString serverFinished)
		{
			var earlySecretResult = GetDerivedEarlySecret(isExternalPsk, preSharedKey, clientHello);
			var handshakeSecretResult = GetDerivedHandshakeSecret(diffieHellmanSharedSecret, earlySecretResult.DerivedEarlySecret, 
				clientHello, serverHello);
			var masterSecretResult = GetDerivedMasterSecret(handshakeSecretResult.DerivedHandshakeSecret, 
				clientHello, clientFinished, serverFinished);
			
			return new TlsKdfV13FullResult()
			{
				EarlySecretResult = earlySecretResult,
				HandshakeSecretResult = handshakeSecretResult,
				MasterSecretResult = masterSecretResult
			};
		}
		
		/// <summary>
		/// Derive-Secret(Secret, Label, Messages) =
		///    HKDF-Expand-Label(Secret, Label, Transcript-Hash(Messages), Hash.length)
		/// </summary>
		/// <param name="secret">The secret.</param>
		/// <param name="label">The label of the intermediate value.</param>
		/// <param name="messages">The context of the KDF step.</param>
		/// <returns>The "otherInfo" for the HKDF, a concatenation of label and hash of messages.</returns>
		private BitString DeriveSecret(BitString secret, string label, params BitString[] messages)
		{
			return ExpandLabel(secret, label, TranscriptHash(messages), _hashOutputLengthBits / 8);
		}

		/// <summary>
		/// HKDF-Expand-Label(Secret, Label, Context, Length) =
		///   HKDF-Expand(Secret, HkdfLabel, Length)
		///
		/// The Hash function used by Transcript-Hash and HKDF is the cipher
		///    suite hash algorithm.  Hash.length is its output length in bytes.
		///    Messages is the concatenation of the indicated handshake messages,
		///    including the handshake message type and length fields, but not
		///    including record layer headers.  Note that in some cases a zero-
		///    length Context (indicated by "") is passed to HKDF-Expand-Label.  The
		///    labels specified in this document are all ASCII strings and do not
		///    include a trailing NUL byte.
		/// </summary>
		/// <param name="secret">The secret.</param>
		/// <param name="label">The label of the intermediate value.</param>
		/// <param name="context">The context of the KDF step.</param>
		/// <param name="hashLengthBytes">Hash output length in bytes.</param>
		/// <returns>HKDF expanded bitstring based on the secret and built other info.</returns>
		public BitString ExpandLabel(BitString secret, string label, BitString context, int hashLengthBytes)
		{
			const int maxLabelContextSizes = 255;
			var expandedLabel = Encoding.ASCII.GetBytes("tls13 " + label);
			
			if (label.Length > maxLabelContextSizes)
			{
				throw new ArgumentOutOfRangeException($"{nameof(label)} exceeds max labels length of {maxLabelContextSizes}");
			}

			if (context.BitLength.CeilingDivide(maxLabelContextSizes) > maxLabelContextSizes)
			{
				throw new ArgumentOutOfRangeException($"{nameof(context)} exceeds max labels length of {maxLabelContextSizes}");
			}

			var otherInfo = BitString.Empty()
				.ConcatenateBits(BitString.To16BitString((short)hashLengthBytes))
				.ConcatenateBits(BitString.To8BitString((byte)expandedLabel.Length))
				.ConcatenateBits(new BitString(expandedLabel))
				.ConcatenateBits(BitString.To8BitString((byte)(context.BitLength / BitString.BITSINBYTE)))
				.ConcatenateBits(context);
			
			return _hkdf.Expand(secret, otherInfo, hashLengthBytes);
		}

		/// <summary>
		/// Transcript-Hash(M1, M2, ... Mn) = Hash(M1 || M2 || ... || Mn)
		/// </summary>
		/// <param name="messages"></param>
		/// <returns></returns>
		public BitString TranscriptHash(params BitString[] messages)
		{
			var concatenatedMessages = BitString.Empty();
			foreach (var message in messages)
			{
				concatenatedMessages = concatenatedMessages.ConcatenateBits(message);
			}
			
			return _sha.HashMessage(concatenatedMessages).Digest;
		}
	}
}