'
' Ejemplo de generar clave SHA1 a partir del nombre y password del usuario
' para el post/artículo en elguillemola.com / elguille.info:
'   Generar clave SHA1 con el nombre y password del usuario
'
' La clase UtilSHA1 define dos métodos estáticos (compartidos):
'   ValidarTextoClave   Comprueba que no se usen caracteres no permitidos
'   GenerarClaveSHA1    Genera un valor SHA1 de 40 caracteres (todo en mayúsculas)
'

Option Strict On
Option Infer On

Imports System

Imports System.Text
Imports System.Security.Cryptography

Module Program
    Sub Main(args As String())
        
        dim valido As Boolean
        dim usuario As String
        dim passw as String

        Do
            Console.Write("Escribe el nombre del usuario: ")
            usuario = Console.ReadLine()
            ' si el nombre del usuario tiene caracteres no permitidos, preguntar de nuevo
            valido = UtilSHA1.ValidarTextoClave(usuario)
            if not valido
                Console.WriteLine("Nombre de usuario NO VÁLIDO.")
            end if
        Loop While Not valido

        Do
            Console.Write("Escribe la clave: ")
            passw = Console.ReadLine()
            ' si la clave tiene caracteres no permitidos, preguntar de nuevo
            valido = UtilSHA1.ValidarTextoClave(passw)
            if not valido
                Console.WriteLine("La clave NO ES VÁLIDA.")
            end if
        Loop While Not valido

        ' generar la clave SHA1 y mostrarla
        dim claveSHA1 = UtilSHA1.GenerarClaveSHA1(usuario, passw)
        Console.WriteLine($"La clave SHA1 es: '{claveSHA1}'.")
            
    End Sub
End Module


public class UtilSHA1

    ''' <summary>
    ''' Generar una clave SHA1 para guardarla en lugar del password,
    ''' de esa forma no se podrá saber la clave.
    ''' La longitud es de 40 caracteres.
    ''' </summary>
    ''' <remarks>28/Mar/2019
    ''' Crear una clave SHA1 como la generada por:
    ''' FormsAuthentication.HashPasswordForStoringInConfigFile
    ''' Basado en el ejemplo de mi sitio:
    ''' http://www.elguille.info/NET/dotnet/comprobar_usuario_usando_base_datos_vb2003.htm
    ''' </remarks>
    Public Shared Function GenerarClaveSHA1(nick As String, clave As String) As String
        ' Crear una clave SHA1 como la generada por 
        ' FormsAuthentication.HashPasswordForStoringInConfigFile
        ' Adaptada del ejemplo de la ayuda en la descripción de SHA1 (Clase)
        Dim enc As New UTF8Encoding
        ' Por si el usuario (nick) es nulo
        If String.IsNullOrWhiteSpace(nick) Then
            nick = ""
        Else
            nick = nick.ToLower
        End If
        Dim data() As Byte = enc.GetBytes(nick & clave)
        Dim result() As Byte

        Dim sha As New SHA1CryptoServiceProvider
        ' This is one implementation of the abstract class SHA1.
        result = sha.ComputeHash(data)

        ' Convertir los valores en hexadecimal
        ' cuando tiene una cifra hay que rellenarlo con cero
        ' para que siempre ocupen dos dígitos.
        Dim sb As New StringBuilder
        For i As Integer = 0 To result.Length - 1
            If result(i) < 16 Then
                sb.Append("0")
            End If
            sb.Append(result(i).ToString("x"))
        Next

        Return sb.ToString.ToUpper
    End Function

    ''' <summary>
    ''' Validar caracteres en la clave.
    ''' No se aceptan ?*%' ni --
    ''' </summary>
    Public Shared Function ValidarTextoClave(laClave As String) As Boolean
        Dim sNoVale As String = "?*%'_"

        laClave = laClave.Trim()

        If laClave.IndexOf("--") > -1 Then
            Return False
        End If
        If laClave.IndexOfAny(sNoVale.ToCharArray) > -1 Then
            Return False
        End If

        Return True
    End Function

end class