//
// Ejemplo de generar clave SHA1 a partir del nombre y password del usuario
// para el post/artículo en elguillemola.com / elguille.info:
//   Generar clave SHA1 con el nombre y password del usuario
//
// La clase UtilSHA1 define dos métodos estáticos (compartidos):
//   ValidarTextoClave   Comprueba que no se usen caracteres no permitidos
//   GenerarClaveSHA1    Genera un valor SHA1 de 40 caracteres (todo en mayúsculas)
//

using System;

using System.Text;
using System.Security.Cryptography;

class Program
{
    static void Main(string[] args)
    {
        bool valido;
        string usuario;
        string passw;

        do
        {
            Console.Write("Escribe el nombre del usuario: ");
            usuario = Console.ReadLine();
            // si el nombre del usuario tiene caracteres no permitidos, preguntar de nuevo
            valido = UtilSHA1.ValidarTextoClave(usuario);
            if (!valido)
                Console.WriteLine("Nombre de usuario NO VÁLIDO.");
        }
        while (!valido);

        do
        {
            Console.Write("Escribe la clave: ");
            passw = Console.ReadLine();
            // si la clave tiene caracteres no permitidos, preguntar de nuevo
            valido = UtilSHA1.ValidarTextoClave(passw);
            if (!valido)
                Console.WriteLine("La clave NO ES VÁLIDA.");
        }
        while (!valido);

        // generar la clave SHA1 y mostrarla
        var claveSHA1 = UtilSHA1.GenerarClaveSHA1(usuario, passw);
        Console.WriteLine($"La clave SHA1 es: '{claveSHA1}'.");
    }
}

public class UtilSHA1
{

    /// <summary>
    /// Generar una clave SHA1 para guardarla en lugar del password,
    /// de esa forma no se podrá saber la clave.
    /// La longitud es de 40 caracteres.
    /// </summary>
    /// <remarks>
    /// Crear una clave SHA1 como la generada por:
    /// FormsAuthentication.HashPasswordForStoringInConfigFile
    /// Basado en el ejemplo de mi sitio:
    /// http://www.elguille.info/NET/dotnet/comprobar_usuario_usando_base_datos_cs2003.htm
    /// </remarks>
    public static string GenerarClaveSHA1(string nick, string clave)
    {
        // Crear una clave SHA1 como la generada por 
        // FormsAuthentication.HashPasswordForStoringInConfigFile
        // Adaptada del ejemplo de la ayuda en la descripción de SHA1 (Clase)
        UTF8Encoding enc = new UTF8Encoding();
        // Por si el usuario (nick) es nulo
        if (string.IsNullOrWhiteSpace(nick))
            nick = "";
        else
            nick = nick.ToLower();
        byte[] data = enc.GetBytes(nick + clave);
        byte[] result;

        SHA1CryptoServiceProvider sha = new SHA1CryptoServiceProvider();
        // This is one implementation of the abstract class SHA1.
        result = sha.ComputeHash(data);

        // Convertir los valores en hexadecimal
        // cuando tiene una cifra hay que rellenarlo con cero
        // para que siempre ocupen dos dígitos.
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i <= result.Length - 1; i++)
        {
            if (result[i] < 16)
                sb.Append("0");
            sb.Append(result[i].ToString("x"));
        }

        return sb.ToString().ToUpper();
    }

    /// <summary>
    /// Validar caracteres en la clave.
    /// No se aceptan ?*%'_ ni --
    /// </summary>
    public static bool ValidarTextoClave(string laClave)
    {
        string sNoVale = "?*%'_";

        laClave = laClave.Trim();

        if (laClave.IndexOf("--") > -1)
            return false;
        if (laClave.IndexOfAny(sNoVale.ToCharArray()) > -1)
            return false;

        return true;
    }
}
