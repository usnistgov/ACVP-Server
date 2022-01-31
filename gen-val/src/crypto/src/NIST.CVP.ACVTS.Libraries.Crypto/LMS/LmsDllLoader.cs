using System;
using System.Runtime.InteropServices;
using NIST.CVP.ACVTS.Libraries.Common.Helpers;

namespace NIST.CVP.ACVTS.Libraries.Crypto.LMS
{
    public static class LmsDllLoader
    {
        #region DllImport

        [DllImport("_dll/LMS.dll", EntryPoint = "gen_priv_lmots")]
        private static extern void gen_priv_lmots_win(int n, int p, byte[] q, byte[] I, byte[] seed, byte[] typecode, IntPtr result);

        [DllImport("_dll/LMS.dylib", EntryPoint = "gen_priv_lmots")]
        private static extern void gen_priv_lmots_osx(int n, int p, byte[] q, byte[] I, byte[] seed, byte[] typecode, IntPtr result);

        [DllImport("_dll/LMS.so", EntryPoint = "gen_priv_lmots")]
        private static extern void gen_priv_lmots_linux(int n, int p, byte[] q, byte[] I, byte[] seed, byte[] typecode, IntPtr result);

        [DllImport("_dll/LMS.dll", EntryPoint = "gen_pub_lmots")]
        private static extern void gen_pub_lmots_win(int n, int p, int w, byte[] private_key, byte[] y, IntPtr result);

        [DllImport("_dll/LMS.dylib", EntryPoint = "gen_pub_lmots")]
        private static extern void gen_pub_lmots_osx(int n, int p, int w, byte[] private_key, byte[] y, IntPtr result);

        [DllImport("_dll/LMS.so", EntryPoint = "gen_pub_lmots")]
        private static extern void gen_pub_lmots_linux(int n, int p, int w, byte[] private_key, byte[] y, IntPtr result);

        [DllImport("_dll/LMS.dll", EntryPoint = "gen_z")]
        private static extern void gen_z_win(int p, int n, int w, byte[] y, byte[] qcksmq, byte[] I, byte[] q, IntPtr z);

        [DllImport("_dll/LMS.dylib", EntryPoint = "gen_z")]
        private static extern void gen_z_osx(int p, int n, int w, byte[] y, byte[] qcksmq, byte[] I, byte[] q, IntPtr z);

        [DllImport("_dll/LMS.so", EntryPoint = "gen_z")]
        private static extern void gen_z_linux(int p, int n, int w, byte[] y, byte[] qcksmq, byte[] I, byte[] q, IntPtr z);

        [DllImport("_dll/LMS.dll", EntryPoint = "gen_sig_lmots")]
        private static extern void gen_sig_lmots_win(int n, int p, int w, int ls, int msgByteLen, byte[] private_key, byte[] seed, byte[] msg, IntPtr result);

        [DllImport("_dll/LMS.dylib", EntryPoint = "gen_sig_lmots")]
        private static extern void gen_sig_lmots_osx(int n, int p, int w, int ls, int msgByteLen, byte[] private_key, byte[] seed, byte[] msg, IntPtr result);

        [DllImport("_dll/LMS.so", EntryPoint = "gen_sig_lmots")]
        private static extern void gen_sig_lmots_linux(int n, int p, int w, int ls, int msgByteLen, byte[] private_key, byte[] seed, byte[] msg, IntPtr result);

        [DllImport("_dll/LMS.dll", EntryPoint = "gen_sig_lmots_nondeterministic")]
        private static extern void gen_sig_lmots_nondeterministic_win(int n, int p, int w, int ls, int msgByteLen, byte[] private_key, byte[] C, byte[] msg, IntPtr result);

        [DllImport("_dll/LMS.dylib", EntryPoint = "gen_sig_lmots_nondeterministic")]
        private static extern void gen_sig_lmots_nondeterministic_osx(int n, int p, int w, int ls, int msgByteLen, byte[] private_key, byte[] C, byte[] msg, IntPtr result);

        [DllImport("_dll/LMS.so", EntryPoint = "gen_sig_lmots_nondeterministic")]
        private static extern void gen_sig_lmots_nondeterministic_linux(int n, int p, int w, int ls, int msgByteLen, byte[] private_key, byte[] C, byte[] msg, IntPtr result);

        [DllImport("_dll/LMS.dll", EntryPoint = "gen_ots_priv_lms")]
        private static extern void gen_ots_priv_lms_win(int h, byte[] I, byte[] seed, int n, int p, byte[] typecode, IntPtr result);

        [DllImport("_dll/LMS.dylib", EntryPoint = "gen_ots_priv_lms")]
        private static extern void gen_ots_priv_lms_osx(int h, byte[] I, byte[] seed, int n, int p, byte[] typecode, IntPtr result);

        [DllImport("_dll/LMS.so", EntryPoint = "gen_ots_priv_lms")]
        private static extern void gen_ots_priv_lms_linux(int h, byte[] I, byte[] seed, int n, int p, byte[] typecode, IntPtr result);

        [DllImport("_dll/LMS.dll", EntryPoint = "build_tree_with_pub")]
        private static extern void build_tree_with_pub_win(int height, int m, byte[] I, byte[] pub, byte[] y, int p, int n, int w, IntPtr tree);

        [DllImport("_dll/LMS.dylib", EntryPoint = "build_tree_with_pub")]
        private static extern void build_tree_with_pub_osx(int height, int m, byte[] I, byte[] pub, byte[] y, int p, int n, int w, IntPtr tree);

        [DllImport("_dll/LMS.so", EntryPoint = "build_tree_with_pub")]
        private static extern void build_tree_with_pub_linux(int height, int m, byte[] I, byte[] pub, byte[] y, int p, int n, int w, IntPtr tree);

        [DllImport("_dll/LMS.dll", EntryPoint = "gen_pub_piece")]
        private static extern void gen_pub_piece_win(long start, long end, int height, int m, byte[] otsPriv, byte[] y, int p, int n, int w, byte[] pub);

        [DllImport("_dll/LMS.dylib", EntryPoint = "gen_pub_piece")]
        private static extern void gen_pub_piece_osx(long start, long end, int height, int m, byte[] otsPriv, byte[] y, int p, int n, int w, byte[] pub);

        [DllImport("_dll/LMS.so", EntryPoint = "gen_pub_piece")]
        private static extern void gen_pub_piece_linux(long start, long end, int height, int m, byte[] otsPriv, byte[] y, int p, int n, int w, byte[] pub);
        #endregion DllImport

        public static byte[] GenPrivLmots(int n, int p, byte[] q, byte[] I, byte[] seed, byte[] typecode)
        {
            void osxAction(IntPtr result) => gen_priv_lmots_osx(n, p, q, I, seed, typecode, result);
            void winAction(IntPtr result) => gen_priv_lmots_win(n, p, q, I, seed, typecode, result);
            void linuxAction(IntPtr result) => gen_priv_lmots_linux(n, p, q, I, seed, typecode, result);

            return DllHelper.PickOS(osxAction, winAction, linuxAction, n * p + 24);
        }

        public static byte[] GenPubLmots(int n, int p, int w, byte[] private_key)
        {
            void osxAction(IntPtr result) => gen_pub_lmots_osx(n, p, w, private_key, new byte[n * p], result);
            void winAction(IntPtr result) => gen_pub_lmots_win(n, p, w, private_key, new byte[n * p], result);
            void linuxAction(IntPtr result) => gen_pub_lmots_linux(n, p, w, private_key, new byte[n * p], result);

            return DllHelper.PickOS(osxAction, winAction, linuxAction, n + 24);
        }

        public static byte[] GenZ(int p, int n, int w, byte[] y, byte[] qcksmq, byte[] I, byte[] q)
        {
            void osxAction(IntPtr result) => gen_z_osx(p, n, w, y, qcksmq, I, q, result);
            void winAction(IntPtr result) => gen_z_win(p, n, w, y, qcksmq, I, q, result);
            void linuxAction(IntPtr result) => gen_z_linux(p, n, w, y, qcksmq, I, q, result);

            return DllHelper.PickOS(osxAction, winAction, linuxAction, p * n);
        }

        public static byte[] GenSigLmots(int n, int p, int w, int ls, int msgByteLen, byte[] private_key, byte[] seed, byte[] msg)
        {
            void osxAction(IntPtr result) => gen_sig_lmots_osx(n, p, w, ls, msgByteLen, private_key, seed, msg, result);
            void winAction(IntPtr result) => gen_sig_lmots_win(n, p, w, ls, msgByteLen, private_key, seed, msg, result);
            void linuxAction(IntPtr result) => gen_sig_lmots_linux(n, p, w, ls, msgByteLen, private_key, seed, msg, result);

            return DllHelper.PickOS(osxAction, winAction, linuxAction, p * n + 36);
        }

        public static byte[] GenSigLmotsNonDeterministic(int n, int p, int w, int ls, int msgByteLen, byte[] private_key, byte[] C, byte[] msg)
        {
            void osxAction(IntPtr result) => gen_sig_lmots_nondeterministic_osx(n, p, w, ls, msgByteLen, private_key, C, msg, result);
            void winAction(IntPtr result) => gen_sig_lmots_nondeterministic_win(n, p, w, ls, msgByteLen, private_key, C, msg, result);
            void linuxAction(IntPtr result) => gen_sig_lmots_nondeterministic_linux(n, p, w, ls, msgByteLen, private_key, C, msg, result);

            return DllHelper.PickOS(osxAction, winAction, linuxAction, p * n + 36);
        }

        public static byte[] GenOtsPrivLms(int h, byte[] I, byte[] seed, int n, int p, byte[] typecode)
        {
            void osxAction(IntPtr result) => gen_ots_priv_lms_osx(h, I, seed, n, p, typecode, result);
            void winAction(IntPtr result) => gen_ots_priv_lms_win(h, I, seed, n, p, typecode, result);
            void linuxAction(IntPtr result) => gen_ots_priv_lms_linux(h, I, seed, n, p, typecode, result);

            return DllHelper.PickOS(osxAction, winAction, linuxAction, (1 << h) * ((n * p) + 24));
        }

        public static byte[] BuildTreeWithPub(int height, int m, byte[] I, byte[] pub, int p, int n, int w)
        {
            void osxAction(IntPtr result) => build_tree_with_pub_osx(height, m, I, pub, new byte[p * n], p, n, w, result);
            void winAction(IntPtr result) => build_tree_with_pub_win(height, m, I, pub, new byte[p * n], p, n, w, result);
            void linuxAction(IntPtr result) => build_tree_with_pub_linux(height, m, I, pub, new byte[p * n], p, n, w, result);

            return DllHelper.PickOS(osxAction, winAction, linuxAction, ((1 << (height + 1)) - 1) * n);
        }

        public static void GenPubPiece(long start, long end, int height, int m, byte[] otsPriv, int p, int n, int w, byte[] pub)
        {
            void osxAction() => gen_pub_piece_osx(start, end, height, m, otsPriv, new byte[p * n], p, n, w, pub);
            void winAction() => gen_pub_piece_win(start, end, height, m, otsPriv, new byte[p * n], p, n, w, pub);
            void linuxAction() => gen_pub_piece_linux(start, end, height, m, otsPriv, new byte[p * n], p, n, w, pub);

            DllHelper.PickOS(osxAction, winAction, linuxAction);
        }
    }
}
