from mpmath import mp

"""
Ensure requirements are installed:

```
pip install -r requirements.txt
```
"""

mp.prec = 10240
mp.dps = 10240

def output_prime_lower_bound(mod):
    """
    Print the mod, and output the hex value for the lower bound value of that mod
    """
    print(section_label(mod))
    print(hex(int(prime_lower_bound(mod))))

def prime_lower_bound(mod):
    """
    Determine the prime number lower bound based on a mod.

    Formula from: https://nvlpubs.nist.gov/nistpubs/FIPS/NIST.FIPS.186-5-draft.pdf
    """
    # floor[sqrt(2) * 2^(mod/2) - 1]
    return mp.floor(mp.sqrt(2) * mp.power(2, mod/2 -1))

def section_label(mod):
    """
    Pretty pretty more or less
    """
    return f"\n-----\n{mod}\n-----\n"

output_prime_lower_bound(1024)
output_prime_lower_bound(2048)
output_prime_lower_bound(8192)
output_prime_lower_bound(15360)