//===========================
// Copyright (c) Tarteeb LLC
// Manage Your Money Easy
//===========================

using System.Threading.Tasks;
using EasyPay.Api.Models.Transfers;

namespace EasyPay.Api.Services.Foundations.Transfers
{
    public partial interface ITransferService
    {
        ValueTask<decimal> MakeTransferAsync(string sourceAccountNumber, string receiverAccountNumber, decimal amount);
        ValueTask<decimal> DepositAsync(string accountNumber, decimal amount);
        ValueTask<decimal> CheckBalanceAsync(string accountNumber);
        ValueTask<Transfer> AddTransferAsync(string receiverAccountNumber, string sourceAccountNumber, decimal amount);
    }
}
