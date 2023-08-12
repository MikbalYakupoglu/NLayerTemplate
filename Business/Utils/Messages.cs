using Entity.Concrete;
using Newtonsoft.Json.Linq;

namespace Business.Utils
{
    public static class Messages
    {
        // User
        public const string EmailCannotBeNull = "Login Boş Bırakılamaz.";
        public const string PasswordCannotBeNull = "Şifre Boş Bırakılamaz.";
        public const string IdNotFound = "Id Bulunamadı.";
        public const string UserNotFound = "Kullanıcı Bulunamadı.";
        public const string IncorrectPassword = "Geçersiz Şifre.";
        public const string RegisterSuccess = "Kayıt Başarılı.";
        public const string RemoveSuccess = "Kullanıcı Başarıyla Silindi.";
        public const string UserAlreadyExists = "Girilen Login Başka Hesap Tarafından Kullanılıyor.";
        public const string NotAllowedToDelete = "Kullanıcı Silmeye Yetkiniz Yok.";

        // User Role
        public const string UserRoleAlreadyExists = "Kullanıcı Rolü Zaten Bulunuyor.";
        public const string UserRoleCreateSuccess = "Kullanıcı Rolü Başarıyla Oluşturuldu.";
        public const string UserRoleNotFound = "Kullanıcı Rolü Bulunamadı.";
        public const string UserRoleDeleteSuccess = "Kullanıcı Rolü Başarıyla Silindi.";

        // User Role Management
        public const string UserRoleUpdateSuccess = "Roller Başarıyla Değiştirildi.";
        public const string UserRoleNotModified = "Rol Değişikliği Bulunamadı.";

        // Role
        public const string RoleNotFound = "Rol Bulunamadı.";
        public const string RoleNameCannotBeNull = "Rol Adı Boş Olamaz.";
        public const string RoleAlreadyExist = "Role Zaten Bulunuyor.";
        public const string RoleCreateSuccess = "Rol Başarıyla Oluşturuldu.";
        public const string RoleUpdateSuccess = "Rol Başarıyla Güncellendi.";
        public const string RoleDeleteSuccess = "Rol Başarıyla Silindi.";
        public const string RoleCannotBeDeleteWhileUsing = "Rol Kullanıcı Tarafından Kullanılırken Silinemez.";
    }
}
