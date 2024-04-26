using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ProjectHK3.Models;

public partial class ProjectHk3Context : DbContext
{
    public ProjectHk3Context()
    {
    }

    public ProjectHk3Context(DbContextOptions<ProjectHk3Context> options)
        : base(options)
    {
    }

    public virtual DbSet<DonDatHang> DonDatHangs { get; set; }

    public virtual DbSet<KhachHang> KhachHangs { get; set; }

    public virtual DbSet<LoaiGiaoHang> LoaiGiaoHangs { get; set; }

    public virtual DbSet<LoaiSanPham> LoaiSanPhams { get; set; }

    public virtual DbSet<NhanVien> NhanViens { get; set; }

    public virtual DbSet<SanPham> SanPhams { get; set; }

    public virtual DbSet<TaiKhoanMatKhau> TaiKhoanMatKhaus { get; set; }

    public virtual DbSet<ThanhToan> ThanhToans { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=DESKTOP-PIJU8TQ\\MSSQLSERVERHK3;Initial Catalog=ProjectHK3;Persist Security Info=True;User ID=sa;Password=123;Encrypt=True;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DonDatHang>(entity =>
        {
            entity.HasKey(e => e.MaDonHang).HasName("PK__DonDatHa__129584ADD2B54629");

            entity.ToTable("DonDatHang", tb => tb.HasTrigger("Trg_DonDatHang_Insert"));

            entity.Property(e => e.MaDonHang).HasMaxLength(16);
            entity.Property(e => e.MaSoSanPham).HasMaxLength(7);
            entity.Property(e => e.NgayDat)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.SoLuongMua).HasDefaultValue(1);
            entity.Property(e => e.ThanhToan)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.MaKhachHangNavigation).WithMany(p => p.DonDatHangs)
                .HasForeignKey(d => d.MaKhachHang)
                .HasConstraintName("FK__DonDatHan__MaKha__4BAC3F29");

            entity.HasOne(d => d.MaLoaiGiaoHangNavigation).WithMany(p => p.DonDatHangs)
                .HasForeignKey(d => d.MaLoaiGiaoHang)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DonDatHan__MaLoa__4D94879B");

            entity.HasOne(d => d.MaSoSanPhamNavigation).WithMany(p => p.DonDatHangs)
                .HasPrincipalKey(p => p.MaSoSanPham)
                .HasForeignKey(d => d.MaSoSanPham)
                .HasConstraintName("FK__DonDatHan__MaSoS__4CA06362");
        });

        modelBuilder.Entity<KhachHang>(entity =>
        {
            entity.HasKey(e => e.MaKhachHang).HasName("PK__KhachHan__88D2F0E597E6F4AE");

            entity.ToTable("KhachHang");

            entity.HasIndex(e => e.MaTaiKhoan, "UQ__KhachHan__AD7C6528FD961571").IsUnique();

            entity.Property(e => e.DiaChi).HasMaxLength(255);
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.SoDienThoai)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.TenKhachHang).HasMaxLength(255);

            entity.HasOne(d => d.MaTaiKhoanNavigation).WithOne(p => p.KhachHang)
                .HasForeignKey<KhachHang>(d => d.MaTaiKhoan)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__KhachHang__MaTai__3F466844");
        });

        modelBuilder.Entity<LoaiGiaoHang>(entity =>
        {
            entity.HasKey(e => e.MaLoaiGiaoHang).HasName("PK__LoaiGiao__3A29720837E801E7");

            entity.ToTable("LoaiGiaoHang");

            entity.Property(e => e.TenLoaiGiaoHang).HasMaxLength(100);
        });

        modelBuilder.Entity<LoaiSanPham>(entity =>
        {
            entity.HasKey(e => e.MaLoai).HasName("PK__LoaiSanP__730A575956468BEF");

            entity.ToTable("LoaiSanPham");

            entity.Property(e => e.TenLoai).HasMaxLength(100);
        });

        modelBuilder.Entity<NhanVien>(entity =>
        {
            entity.HasKey(e => e.MaNhanVien).HasName("PK__NhanVien__77B2CA4776DF00E2");

            entity.ToTable("NhanVien");

            entity.HasIndex(e => e.MaTaiKhoan, "UQ__NhanVien__AD7C65282ED753C7").IsUnique();

            entity.Property(e => e.TenNhanVien).HasMaxLength(255);

            entity.HasOne(d => d.MaTaiKhoanNavigation).WithOne(p => p.NhanVien)
                .HasForeignKey<NhanVien>(d => d.MaTaiKhoan)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__NhanVien__MaTaiK__3B75D760");
        });

        modelBuilder.Entity<SanPham>(entity =>
        {
            entity.HasKey(e => e.MaSanPham).HasName("PK__SanPham__FAC7442D627EF7A3");

            entity.ToTable("SanPham", tb =>
                {
                    tb.HasTrigger("Trg_SanPham_Insert");
                    tb.HasTrigger("Trg_SanPham_Update");
                });

            entity.HasIndex(e => e.MaSoSanPham, "UQ__SanPham__F23215377698332F").IsUnique();

            entity.Property(e => e.Gia).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.MaSoSanPham).HasMaxLength(7);
            entity.Property(e => e.MoTaSanPham).HasMaxLength(255);
            entity.Property(e => e.TenSanPham).HasMaxLength(255);
        });

        modelBuilder.Entity<TaiKhoanMatKhau>(entity =>
        {
            entity.HasKey(e => e.MaTaiKhoan).HasName("PK__TaiKhoan__AD7C6529DF0930EB");

            entity.ToTable("TaiKhoanMatKhau");

            entity.HasIndex(e => e.TaiKhoan, "UQ__TaiKhoan__D5B8C7F01A4C9782").IsUnique();

            entity.Property(e => e.MatKhau)
                .HasMaxLength(256)
                .IsUnicode(false);
            entity.Property(e => e.TaiKhoan)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<ThanhToan>(entity =>
        {
            entity.HasKey(e => e.MaThanhToan).HasName("PK__ThanhToa__D4B25844BFC8C8DE");

            entity.ToTable("ThanhToan");

            entity.Property(e => e.MaDonHang).HasMaxLength(16);
            entity.Property(e => e.PhuongThucThanhToan)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.ThoiGian)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.MaDonHangNavigation).WithMany(p => p.ThanhToans)
                .HasForeignKey(d => d.MaDonHang)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ThanhToan__MaDon__5165187F");

            entity.HasOne(d => d.MaKhachHangNavigation).WithMany(p => p.ThanhToans)
                .HasForeignKey(d => d.MaKhachHang)
                .HasConstraintName("FK__ThanhToan__MaKha__52593CB8");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
