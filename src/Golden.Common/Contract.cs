using System;

namespace Golden.Common
{
    public class Contract
    {
        public static ArgumentContract Requires(bool condition)
            => _Requires(name: null, condition);
        public static ArgumentContract Requires(string name, bool condition)
            => _Requires(name, condition);
        private static ArgumentContract _Requires(string? name, bool condition)
        {
            return new ArgumentContract(condition, name);
        }
    }

    public class ArgumentContract
    {
        private readonly bool _condition;
        private readonly string? _name;

        internal ArgumentContract(bool condition, string? name)
        {
            _condition = condition;
            _name = name;
        }

        public void OnFailureThrow()
        {
            if (_name != null)
                OnFailureThrow($"{_name} has invalid value");
            else
                throw new Exception();
        }

        public void OnFailureThrow(string message)
        {
            if (_condition == false)
                throw new Exception(message);
        }

        public void OnFailureThrow(Exception exception)
        {
            if (_condition == false)
                throw exception;
        }

        public void OnFailureThrow<TException>() where TException : Exception, new()
        {
            if (_condition == false)
                throw new TException();
        }
    }
}
