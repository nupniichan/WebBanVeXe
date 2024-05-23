//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ltweb_.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public partial class NhanVien
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public NhanVien()
        {
            this.ChuyenDi = new HashSet<ChuyenDi>();
        }

        public int IdNhanVien { get; set; }
        [DisplayName("Họ tên")]
        [Required(ErrorMessage = "Vui lòng nhập họ tên")]
        public string HoTen { get; set; }
        [DisplayName("Ngày sinh")]
        [Required(ErrorMessage = "Vui lòng nhập ngày sinh")]
        public Nullable<System.DateTime> NgaySinh { get; set; }
        [DisplayName("Số điện thoại")]
        [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        public Nullable<int> SoDienThoai { get; set; }
        [DisplayName("Chức vụ")]
        [Required(ErrorMessage = "Vui lòng nhập chức vụ")]
        public string ChucVu { get; set; }
        [DisplayName("Email")]
        [Required(ErrorMessage = "Vui lòng nhập email")]
        public string email { get; set; }
        public Nullable<int> IdHangXe { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ChuyenDi> ChuyenDi { get; set; }
        public virtual HangXe HangXe { get; set; }
    }
}
