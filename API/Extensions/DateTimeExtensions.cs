
namespace API.Extensions
{
    public static class DateTimeExtensions
    {

        public static int CalculateAge(this DateOnly dob)
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow); //obtengo fecha actual

            var age = today.Year - dob.Year; //calculo diferencia de año entre el actual year y la fecha de necimiento

            if (dob.AddYears(age) > today) //si mi fecha de nacimiento + mi edad calculada es mayor al dia actual
            {
                age--;
            }

            return age;
        }
    }
}