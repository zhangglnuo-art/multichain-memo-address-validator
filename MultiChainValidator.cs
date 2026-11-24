using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AllSwap.Tools
{
    public class ChainRule
    {
        public string Code;
        public string Name;
        public string Pattern;
        public bool RequiresMemo;
        public string MemoName;
        public string MemoPattern;

        public ChainRule(string code, string name, string pattern, bool requiresMemo, string memoName = "", string memoPattern = "")
        {
            Code = code;
            Name = name;
            Pattern = pattern;
            RequiresMemo = requiresMemo;
            MemoName = memoName;
            MemoPattern = memoPattern;
        }
    }

    class Program
    {
        static readonly Dictionary<string, ChainRule> Rules = new Dictionary<string, ChainRule>(StringComparer.OrdinalIgnoreCase)
        {
            { "BTC", new ChainRule("BTC", "Bitcoin", @"^(bc1[ac-hj-np-z02-9]{11,71}|[13][a-km-zA-HJ-NP-Z1-9]{25,34})$", false) },
            { "ETH", new ChainRule("ETH", "Ethereum (EVM)", @"^0x[a-fA-F0-9]{40}$", false) },
            { "ARB", new ChainRule("ARB", "Arbitrum One", @"^0x[a-fA-F0-9]{40}$", false) },
            { "OP", new ChainRule("OP", "Optimism", @"^0x[a-fA-F0-9]{40}$", false) },
            { "BASE", new ChainRule("BASE", "Base", @"^0x[a-fA-F0-9]{40}$", false) },
            { "MATIC", new ChainRule("MATIC", "Polygon", @"^0x[a-fA-F0-9]{40}$", false) },
            { "BSC", new ChainRule("BSC", "BNB Smart Chain", @"^0x[a-fA-F0-9]{40}$", false) },
            { "AVAX", new ChainRule("AVAX", "Avalanche C-Chain", @"^0x[a-fA-F0-9]{40}$", false) },
            { "MANTLE", new ChainRule("MANTLE", "Mantle", @"^0x[a-fA-F0-9]{40}$", false) },
            { "LINEA", new ChainRule("LINEA", "Linea", @"^0x[a-fA-F0-9]{40}$", false) },
            { "SCROLL", new ChainRule("SCROLL", "Scroll", @"^0x[a-fA-F0-9]{40}$", false) },
            { "ZKSYNC", new ChainRule("ZKSYNC", "zkSync Era", @"^0x[a-fA-F0-9]{40}$", false) },
            { "BLAST", new ChainRule("BLAST", "Blast", @"^0x[a-fA-F0-9]{40}$", false) },
            { "FTM", new ChainRule("FTM", "Fantom", @"^0x[a-fA-F0-9]{40}$", false) },
            { "GNOSIS", new ChainRule("GNOSIS", "Gnosis", @"^0x[a-fA-F0-9]{40}$", false) },
            { "CRO", new ChainRule("CRO", "Cronos", @"^0x[a-fA-F0-9]{40}$", false) },
            { "SOL", new ChainRule("SOL", "Solana", @"^[1-9A-HJ-NP-Za-km-z]{32,44}$", false) },
            { "TRX", new ChainRule("TRX", "TRON", @"^T[1-9A-HJ-NP-Za-km-z]{33}$", false) },
            { "XRP", new ChainRule("XRP", "XRP Ledger", @"^r[1-9A-HJ-NP-Za-km-z]{24,34}$", true, "Destination Tag", @"^\d{1,10}$") },
            { "XLM", new ChainRule("XLM", "Stellar", @"^G[A-Z2-7]{55}$", true, "Memo", @"^[a-zA-Z0-9\-_]{1,28}$") },
            { "TON", new ChainRule("TON", "TON Network", @"^(EQ|UQ)[A-Za-z0-9_\-]{46}$", true, "Comment/Memo", @"^.{1,120}$") },
            { "ATOM", new ChainRule("ATOM", "Cosmos Hub", @"^cosmos1[ac-hj-np-z02-9]{38}$", true, "Memo", @"^.{1,256}$") },
            { "OSMO", new ChainRule("OSMO", "Osmosis", @"^osmo1[ac-hj-np-z02-9]{38}$", true, "Memo", @"^.{1,256}$") },
            { "INJ", new ChainRule("INJ", "Injective", @"^inj1[ac-hj-np-z02-9]{38}$", true, "Memo", @"^.{1,256}$") },
            { "KAVA", new ChainRule("KAVA", "Kava", @"^kava1[ac-hj-np-z02-9]{38}$", true, "Memo", @"^.{1,256}$") },
            { "EOS", new ChainRule("EOS", "EOS Network", @"^[a-z1-5\.]{1,12}$", true, "Memo", @"^.{1,256}$") },
            { "HBAR", new ChainRule("HBAR", "Hedera", @"^0\.0\.\d{1,10}$", true, "Memo", @"^.{1,100}$") },
            { "SUI", new ChainRule("SUI", "Sui", @"^0x[a-fA-F0-9]{64}$", false) },
            { "APT", new ChainRule("APT", "Aptos", @"^0x[a-fA-F0-9]{64}$", false) },
            { "NEAR", new ChainRule("NEAR", "NEAR Protocol", @"^([a-f0-9]{64}|[a-z0-9_\-]{2,64}\.near)$", false) },
            { "DOT", new ChainRule("DOT", "Polkadot", @"^1[a-km-zA-HJ-NP-Z1-9]{46,47}$", false) },
            { "ADA", new ChainRule("ADA", "Cardano", @"^addr1[ac-hj-np-z02-9]{50,105}$", false) },
            { "LTC", new ChainRule("LTC", "Litecoin", @"^(ltc1[ac-hj-np-z02-9]{39,59}|[LM3][a-km-zA-HJ-NP-Z1-9]{26,33})$", false) },
            { "DOGE", new ChainRule("DOGE", "Dogecoin", @"^D[5-9A-HJ-NP-U][1-9A-HJ-NP-Za-km-z]{32}$", false) },
            { "BCH", new ChainRule("BCH", "Bitcoin Cash", @"^((bitcoincash:)?(q|p)[a-z0-9]{41}|[13][a-km-zA-HJ-NP-Z1-9]{25,34})$", false) },
            { "XMR", new ChainRule("XMR", "Monero", @"^[48][0-9AB][1-9A-HJ-NP-Za-km-z]{93}$", false) },
            { "SEI", new ChainRule("SEI", "Sei Network", @"^(sei1[ac-hj-np-z02-9]{38}|0x[a-fA-F0-9]{40})$", true, "Memo (for native Sei)", @"^.{1,256}$") }
        };

        static int Main(string[] args)
        {
            if (args.Length == 0 || Array.IndexOf(args, "--help") >= 0 || Array.IndexOf(args, "-h") >= 0)
            {
                PrintHelp();
                return 0;
            }

            if (Array.IndexOf(args, "--list-chains") >= 0)
            {
                Console.WriteLine("Supported Blockchains (AllSwap MultiChain Validator v1.0.0 - https://allswap.io/tools/address-validator):");
                foreach (var kv in Rules)
                {
                    Console.WriteLine(string.Format("  {0,-8} | {1,-22} | Memo Required: {2}", kv.Value.Code, kv.Value.Name, kv.Value.RequiresMemo ? "YES (" + kv.Value.MemoName + ")" : "No"));
                }
                return 0;
            }

            int poisonIdx = Array.IndexOf(args, "--check-poison");
            if (poisonIdx >= 0 && poisonIdx + 2 < args.Length)
            {
                return CheckPoisoning(args[poisonIdx + 1], args[poisonIdx + 2]);
            }

            string chain = GetArg(args, "--chain");
            string address = GetArg(args, "--address");
            string memo = GetArg(args, "--memo");

            if (string.IsNullOrEmpty(chain) || string.IsNullOrEmpty(address))
            {
                Console.Error.WriteLine("Error: Both --chain <CODE> and --address <ADDR> are required.");
                PrintHelp();
                return 1;
            }

            ChainRule rule;
            if (!Rules.TryGetValue(chain, out rule))
            {
                Console.Error.WriteLine("Error: Unsupported chain code '" + chain + "'. Run with --list-chains to view all supported chains.");
                return 1;
            }

            bool addressValid = Regex.IsMatch(address, rule.Pattern);
            Console.WriteLine("Chain:         " + rule.Name + " (" + rule.Code + ")");
            Console.WriteLine("Address:       " + address);
            Console.WriteLine("Format Valid:  " + (addressValid ? "PASS" : "FAIL"));

            if (!addressValid)
            {
                Console.WriteLine("Result:        INVALID_ADDRESS_FORMAT");
                return 2;
            }

            if (rule.RequiresMemo)
            {
                if (string.IsNullOrEmpty(memo))
                {
                    Console.WriteLine("Memo Status:   WARNING - " + rule.Name + " custodial deposits require a " + rule.MemoName + " (--memo)!");
                }
                else
                {
                    bool memoValid = Regex.IsMatch(memo, rule.MemoPattern);
                    Console.WriteLine("Memo (" + rule.MemoName + "): " + memo + " -> " + (memoValid ? "VALID" : "INVALID_MEMO_FORMAT"));
                    if (!memoValid) return 3;
                }
            }

            Console.WriteLine("Result:        VALID");
            return 0;
        }

        static int CheckPoisoning(string addr1, string addr2)
        {
            if (string.Equals(addr1, addr2, StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Result: IDENTICAL_ADDRESSES (Safe)");
                return 0;
            }
            if (addr1.Length >= 12 && addr2.Length >= 12)
            {
                string p1 = addr1.Substring(0, 6).ToLowerInvariant();
                string p2 = addr2.Substring(0, 6).ToLowerInvariant();
                string s1 = addr1.Substring(addr1.Length - 4).ToLowerInvariant();
                string s2 = addr2.Substring(addr2.Length - 4).ToLowerInvariant();
                if (p1 == p2 && s1 == s2)
                {
                    Console.WriteLine("ALERT: HIGH ADDRESS-POISONING RISK! Prefix (" + p1 + ") and suffix (" + s1 + ") match, but middle bytes differ!");
                    return 4;
                }
            }
            Console.WriteLine("Result: DISTINCT_ADDRESSES (No lookalike prefix/suffix match)");
            return 0;
        }

        static string GetArg(string[] args, string flag)
        {
            int idx = Array.IndexOf(args, flag);
            if (idx >= 0 && idx + 1 < args.Length) return args[idx + 1];
            return null;
        }

        static void PrintHelp()
        {
            Console.WriteLine("AllSwap MultiChain Memo & Address Validator v1.0.0 (MIT License)");
            Console.WriteLine("Homepage: https://allswap.io/tools/address-validator | https://allswap.io");
            Console.WriteLine();
            Console.WriteLine("Usage:");
            Console.WriteLine("  multichain-address-validator.exe --chain <CHAIN> --address <ADDRESS> [--memo <TAG>]");
            Console.WriteLine("  multichain-address-validator.exe --check-poison <ADDRESS_A> <ADDRESS_B>");
            Console.WriteLine("  multichain-address-validator.exe --list-chains");
        }
    }
}
