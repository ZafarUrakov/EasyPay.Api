using System;
using Xeptions;

namespace EasyPay.Api.Models.Transfers.Exceptions
{
    public class TransferServiceException : Xeption
    {
        public TransferServiceException(Exception innerException)
            : base(message: "Transfer service error occurred, contact support.", innerException)
        { }
    }
}
