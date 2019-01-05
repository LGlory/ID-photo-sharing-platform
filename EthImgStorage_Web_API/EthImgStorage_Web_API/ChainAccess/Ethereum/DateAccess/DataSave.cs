﻿using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
using System;
using System.Threading.Tasks;

namespace ChainAccess.Ethereum.DateAccess
{
    public class DataSave : EthereumBase
    {
        public Account Account { get; set; }

        public DataSave()
        {
            Account = new Account();
        }

        public async Task<string> SaveDataToChain(string data, string address, string password)
        {
            var haxdata = new HexUTF8String(data).HexValue;
            if (haxdata.Length/2 > 31744)
            {
                throw new Exception("The data can't be larger than 32KB!");
            }
            var input = new TransactionInput(haxdata, new HexBigInteger("0xffffff"), address);
            var isSuccessful = await Account.UnlockAccountAsync(address, password);
            if (!isSuccessful)
            {
                throw new Exception("Account or Password wrong!");
            }
            var resultHash = await Web3.Eth.Transactions
                .SendTransaction
                .SendRequestAsync(input);
            return resultHash;
        }
    }
}
