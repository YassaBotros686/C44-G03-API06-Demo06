using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Shared.CommonResult
{
    public class Result
    {
        // IsSuccess
        // IsFailure
        // Errors [Code - Description - Type] => Class

        protected readonly List<Error> _errors = [];

        public bool IsSuccess => _errors.Count() == 0;
        public bool IsFailure => !IsSuccess;

        // Property to get Errors

        public IReadOnlyList<Error> Errors => _errors;

        // No Errors =>

        protected Result() { }

        // One Error Occurred =>

        protected Result(Error error)
        {
            _errors.Add(error);
        }

        // More than one Error Occurred =>

        protected Result(List<Error> errors)
        {
            _errors.AddRange(errors);
        }

        // Methods to set Values in CTORs

        // If There is no Errors
        public static Result Ok()
        {
            return new Result();
        }
        // If There is one Error
        public static Result Fail(Error error)
        {
            return new Result(error);
        }
        // If There is more than one Error
        public static Result Fail(List<Error> errors)
        {
            return new Result(errors);
        }
    }

    public class Result<TValue> : Result
    {
        private readonly TValue _value;
        public TValue Value => IsSuccess ? _value : throw new InvalidOperationException("Cannot access the value of a failure result.");

        public Result(TValue value)
        {
            _value = value;
        }
        public Result(Error error):base(error)
        {
            _value = default!;
        }
        public Result(List<Error> errors):base(errors)
        {
            _value = default!;
        }

        // Static Factory Methods
        public static Result<TValue> Ok(TValue value) => new Result<TValue>(value);
        public static new Result<TValue> Fail(Error error) => new Result<TValue>(error);
        public static new Result<TValue> Fail(List<Error> errors) => new Result<TValue>(errors);
        
        // Implicit Casting
        public static implicit operator Result<TValue>(TValue value) => Ok(value);
        public static implicit operator Result<TValue>(Error error) => Fail(error);
        public static implicit operator Result<TValue>(List<Error> errors) => Fail(errors);
    }
}
