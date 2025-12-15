
using StudentWepApi.Data.StudentContext;

namespace StudentWepApi.Utility
{
    public class Commonn
    {
        public static int CalculateAge(DateOnly? dob)
        {
            if (dob == null)
            {
                throw new ArgumentNullException(nameof(dob), "Date of birth cannot be null.");
            }
            var today = DateOnly.FromDateTime(DateTime.Today);
            var birthDate = dob.Value;
            int age = today.Year - birthDate.Year;
            if (birthDate > today.AddYears(-age)) age--;
            return age;
        }

        public static bool IsEmptyFirstName(string? firstName)
        {
            return !string.IsNullOrWhiteSpace(firstName);
        }

        public static bool IsEmptyLastName(string? lastName)
        {
            return !string.IsNullOrWhiteSpace(lastName);
        }


        public static bool IsValidMobile(string? mobile)
        {
            if (string.IsNullOrWhiteSpace(mobile)) return false;

            var mobilePattern = @"^[6-9]\d{9}$";
            return System.Text.RegularExpressions.Regex.IsMatch(mobile, mobilePattern);
        }

        public static bool IsValidEmail(string? email)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;

            var emailPattern = @"^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            return System.Text.RegularExpressions.Regex.IsMatch(email, emailPattern);
        }

        //public static bool FileExists(string sourceFile)
        //{
        //    if (string.IsNullOrWhiteSpace(sourceFile))
        //        throw new ArgumentException("Source file path cannot be null or empty.", nameof(sourceFile));

        //    return File.Exists(sourceFile);
        //}

        //public static bool DirectoryExists(string destinationDirectory)
        //{
        //    if (string.IsNullOrWhiteSpace(destinationDirectory))
        //        throw new ArgumentException("Destination directory path cannot be null or empty.", nameof(destinationDirectory));

        //    return Directory.Exists(destinationDirectory);
        //}

        //public static bool FileDelete(string filePath)
        //{
        //    if (string.IsNullOrWhiteSpace(filePath))
        //        throw new ArgumentException("File path cannot be null or empty.", nameof(filePath));

        //    if (File.Exists(filePath))
        //    {
        //        try
        //        {
        //            File.Delete(filePath);
        //            return true;
        //        }
        //        catch (Exception)
        //        {
        //            return false;
        //        }
        //    }
        //    return false;
        //}


        //public static bool DirectoryDelete(string directoryPath)
        //{
        //    if (string.IsNullOrWhiteSpace(directoryPath))
        //        throw new ArgumentException("Directory path cannot be null or empty.", nameof(directoryPath));

        //    if (Directory.Exists(directoryPath))
        //    {
        //        try
        //        {
        //            Directory.Delete(directoryPath, recursive: true);
        //            return true;
        //        }
        //        catch (Exception)
        //        {
        //            return false;
        //        }
        //    }
        //    return false;
        //}


        //public static bool IsDirectoryEmpty(string destinationDirectory)
        //{
        //    if (string.IsNullOrWhiteSpace(destinationDirectory))
        //        throw new ArgumentException("Directory path cannot be null or empty.", nameof(destinationDirectory));

        //    return Directory.Exists(destinationDirectory) && !Directory.EnumerateFileSystemEntries(destinationDirectory).Any();
        //}

    }
}
