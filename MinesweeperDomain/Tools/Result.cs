using System;

namespace Minesweeper.Tools
{
    public class Result
    {
        public bool Success { get; private set; }
        public string Error { get; private set; }

        public bool Failure
        {
            get { return !Success; }
        }

        protected Result(bool success, string error)
        {
            if (!(success || !string.IsNullOrEmpty(error))) throw new ApplicationException("Not allowed");
            if (!(!success || string.IsNullOrEmpty(error))) throw new ApplicationException("Not allowed");

            Success = success;
            Error = error;
        }

        public static Result Fail(string message)
        {
            return new Result(false, message);
        }

        public static Result<T> Fail<T>(string message)
        {
            return new Result<T>(default, false, message);
        }

        public static Result Ok()
        {
            return new Result(true, String.Empty);
        }

        public static Result<T> Ok<T>(T value)
        {
            return new Result<T>(value, true, String.Empty);
        }

        public static Result Combine(params Result[] results)
        {
            foreach (Result result in results)
            {
                if (result.Failure)
                    return result;
            }

            return Ok();
        }
    }


    public class Result<T> : Result
    {
        private T _value;

        public T Value
        {
            get
            {
                if (!Success) throw new ApplicationException("Not allowed");
                return _value;
            }

            private set { _value = value; }
        }

        protected internal Result(T value, bool success, string error) : base(success, error)
        {
            if (!(value != null || !success)) throw new ApplicationException("Not allowed");
            Value = value;
        }
    }
}