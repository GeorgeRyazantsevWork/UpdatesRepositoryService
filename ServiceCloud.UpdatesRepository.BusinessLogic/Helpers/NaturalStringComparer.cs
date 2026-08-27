using System.Globalization;

namespace ServiceCloud.UpdatesRepository.BusinessLogic.Helpers
{
    /// <summary>
    /// Сравнение строк с учетом содержащихся в них чисел
    /// </summary>
    public class NaturalStringComparer : IComparer<string>
    {
        /// <summary>
        /// Экземпляр сравнивающего объекта
        /// </summary>
        public static NaturalStringComparer Instance { get; } = new NaturalStringComparer();

        /// <inheritdoc />
        public int Compare(string? x, string? y)
        {
            if (!ReferenceEquals(x, null) && ReferenceEquals(y, null))
            {
                return 1;
            }
            else if (ReferenceEquals(x, null) && !ReferenceEquals(y, null))
            {
                return -1;
            }
            else if (ReferenceEquals(x, null) && ReferenceEquals(y, null))
            {
                return 0;
            }

            const int LeftIsSmaller = -1;
            const int RightIsSmaller = 1;
            const int Equal = 0;

            string leftString = x ?? "";
            string rightString = y ?? "";

            var stringComparer = CultureInfo.CurrentCulture.CompareInfo;

            int rightIndex;
            int leftIndex;

            for (leftIndex = 0, rightIndex = 0;
                 leftIndex < leftString.Length && rightIndex < rightString.Length;
                 leftIndex++, rightIndex++)
            {
                var leftChar = leftString[leftIndex];
                var rightChar = rightString[leftIndex];

                var leftIsNumber = char.IsNumber(leftChar);
                var rightIsNumber = char.IsNumber(rightChar);

                if (!leftIsNumber && !rightIsNumber)
                {
                    var result = stringComparer.Compare(leftString, leftIndex, 1, rightString, leftIndex, 1);
                    if (result != 0) return result;
                }
                else if (leftIsNumber && !rightIsNumber)
                {
                    return LeftIsSmaller;
                }
                else if (!leftIsNumber && rightIsNumber)
                {
                    return RightIsSmaller;
                }
                else
                {
                    var leftNumberLength = NumberLength(leftString, leftIndex, out var leftNumber);
                    var rightNumberLength = NumberLength(rightString, rightIndex, out var rightNumber);

                    if (leftNumberLength < rightNumberLength)
                    {
                        return LeftIsSmaller;
                    }
                    else if (leftNumberLength > rightNumberLength)
                    {
                        return RightIsSmaller;
                    }
                    else
                    {
                        if (leftNumber < rightNumber)
                        {
                            return LeftIsSmaller;
                        }
                        else if (leftNumber > rightNumber)
                        {
                            return RightIsSmaller;
                        }
                    }
                }
            }

            if (leftString.Length < rightString.Length)
            {
                return LeftIsSmaller;
            }
            else if (leftString.Length > rightString.Length)
            {
                return RightIsSmaller;
            }

            return Equal;
        }

        /// <summary>
        /// Определение длины числа в строке символов
        /// </summary>
        /// <param name="str">Строка текста</param>
        /// <param name="offset">Начальная позиция для поиска числа в строке</param>
        /// <param name="number">Найденное число</param>
        /// <returns>TRUE - строка содержит число, FALSE - строка не содержит числа</returns>
        /// <exception cref="ArgumentNullException">Не задана строка для поиска</exception>
        /// <exception cref="ArgumentOutOfRangeException">Начальная позиция для поиска выходит за границы строки</exception>
        /// <exception cref="ArgumentException">Проверяемая подстрока не является числом</exception>
        public int NumberLength(string str, int offset, out int number)
        {
            if (string.IsNullOrWhiteSpace(str))
                throw new ArgumentNullException(nameof(str));

            if (offset >= str.Length)
                throw new ArgumentOutOfRangeException(nameof(offset), offset, "Offset must be less than the length of the string.");

            var currentOffset = offset;

            var curChar = str[currentOffset];

            if (!char.IsNumber(curChar))
                throw new ArgumentException($"'{curChar}' is not a number.", nameof(offset));

            int length = 1;

            var numberString = string.Empty;

            for (currentOffset = offset;
                currentOffset < str.Length;
                currentOffset++, length++)
            {

                curChar = str[currentOffset];
                if (!char.IsNumber(curChar))
                {
                    number = int.Parse(numberString);

                    return length;
                }

                numberString += curChar;
            }

            number = int.Parse(numberString);

            return length;
        }
    }
}