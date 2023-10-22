//===========================
// Copyright (c) Tarteeb LLC
// Manage Your Money Easy
//===========================

using EasyPay.Api.Models.Transfers;
using System.Threading.Tasks;

namespace EasyPay.Api.Services.Foundations.Transfers
{
    public partial interface ITransferService
    {
        ValueTask<decimal> MakeTransferAsync(
            string sourceAccountNumber, string receiverAccountNumber, decimal amount);
        ValueTask<decimal> DepositAsync(string accountNumber, decimal amount);
        ValueTask<decimal> CheckBalanceAsync(string accountNumber);
        ValueTask<Transfer> AddTransferAsync(
            string receiverAccountNumber, string sourceAccountNumber, decimal amount);
    }
}
