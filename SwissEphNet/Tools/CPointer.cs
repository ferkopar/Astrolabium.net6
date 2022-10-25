using System.Linq;

namespace SwissEphNet.Tools
{

    /// <summary>
    /// Simulate a C pointer
    /// </summary>
    public struct CPointer<T>
    {
        private readonly T[] _baseArray;
        private readonly int _baseIndex;
        /// <summary>
        /// Create new struct
        /// </summary>
        public CPointer(T[] baseArray)
            : this(baseArray, 0) {
        }

        /// <summary>
        /// Create new struct
        /// </summary>
        public CPointer(T[] baseArray, int baseIndex)
            : this() {
            _baseArray = baseArray;
            _baseIndex = baseIndex;
        }

        /// <summary>
        /// HashCode
        /// </summary>
        public override int GetHashCode() => (_baseArray != null ? _baseArray.GetHashCode() : 0) ^ _baseIndex.GetHashCode();

        /// <summary>
        /// Determine if <paramref name="obj"/> is equals with this one
        /// </summary>
        public override bool Equals(object obj) {
            if (obj is T[]) {
                return _baseArray == obj && _baseIndex == 0;
            } else if (obj is CPointer<T>) {
                return _baseArray == ((CPointer<T>)obj)._baseArray && _baseIndex == ((CPointer<T>)obj)._baseIndex;
            }
            return base.Equals(obj);
        }
        /// <summary>
        /// Read value pointed by access
        /// </summary>
        public static implicit operator T(CPointer<T> array) {
            return array._baseArray[array._baseIndex];
        }

        /// <summary>
        /// Implicit conversion of an array to an ArrayAccess
        /// </summary>
        public static implicit operator CPointer<T>(T[] array) {
            return new CPointer<T>(array);
        }

        /// <summary>
        /// Explicit converision of an ArrayAccess to an array
        /// </summary>
        public static explicit operator T[](CPointer<T> array) {
            return array._baseArray == null ? null : array._baseArray.Skip(array._baseIndex).ToArray();
        }

        /// <summary>
        /// Increment an array access 'pointer'
        /// </summary>
        public static CPointer<T> operator +(CPointer<T> array, int offset) {
            return new CPointer<T>(array._baseArray, array._baseIndex + offset);
        }

        /// <summary>
        /// Decrement an array access 'pointer'
        /// </summary>
        public static CPointer<T> operator -(CPointer<T> array, int offset) {
            return new CPointer<T>(array._baseArray, array._baseIndex - offset);
        }

        /// <summary>
        /// Increment an array access 'pointer'
        /// </summary>
        public static CPointer<T> operator ++(CPointer<T> array) {
            return new CPointer<T>(array._baseArray, array._baseIndex + 1);
        }

        /// <summary>
        /// Decrement an array access 'pointer'
        /// </summary>
        public static CPointer<T> operator --(CPointer<T> array) {
            return new CPointer<T>(array._baseArray, array._baseIndex - 1);
        }

        /// <summary>
        /// Test if an inner array is the same of an array
        /// </summary>
        public static bool operator ==(CPointer<T> access, T[] array) {
            return access._baseArray == array && access._baseIndex == 0;
        }

        /// <summary>
        /// Test if an inner array is not the same of an array
        /// </summary>
        public static bool operator !=(CPointer<T> access, T[] array) {
            return access._baseArray != array && access._baseIndex == 0;
        }

        /// <summary>
        /// Test if an inner array is the same of an array
        /// </summary>
        public static bool operator ==(T[] array, CPointer<T> access) {
            return access._baseArray == array && access._baseIndex == 0;
        }

        /// <summary>
        /// Test if an inner array is not the same of an array
        /// </summary>
        public static bool operator !=(T[] array, CPointer<T> access) {
            return access._baseArray != array && access._baseIndex == 0;
        }

        /// <summary>
        /// Test two pointers equality
        /// </summary>
        public static bool operator ==(CPointer<T> p1, CPointer<T> p2) {
            return p1._baseArray == p2._baseArray && p1._baseIndex == p2._baseIndex;
        }

        /// <summary>
        /// Test two pointers inequality
        /// </summary>
        public static bool operator !=(CPointer<T> p1, CPointer<T> p2) {
            return p1._baseArray != p2._baseArray || p1._baseIndex != p2._baseIndex;
        }

        /// <summary>
        /// Inform if this AccessArray is null
        /// </summary>
        public bool IsNull => _baseArray == null;

        /// <summary>
        /// Get or set the array values
        /// </summary>
        public T this[int idx] {
            get => _baseArray[_baseIndex + idx];
            set => _baseArray[_baseIndex + idx] = value;
        }

        /// <summary>
        /// Length
        /// </summary>
        public int Length => _baseArray?.Length - _baseIndex ?? 0;
    }

}
