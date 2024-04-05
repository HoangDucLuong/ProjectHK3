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

    public virtual DbSet<CodeSanPham> CodeSanPhams { get; set; }

    public virtual DbSet<DonDatHang> DonDatHangs { get; set; }

    public virtual DbSet<KhachHang> KhachHangs { get; set; }

    public virtual DbSet<NhanVien> NhanViens { get; set; }

    public virtual DbSet<SanPham> SanPhams { get; set; }

    public virtual DbSet<ThanhToan> ThanhToans { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=DESKTOP-RQ8HM1R;Initial Catalog=ProjectHK3;Persist Security Info=True;User ID=sa;Password=123;Encrypt=True;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CodeSanPham>(entity =>
        {
            entity.HasKey(e => e.MaCode).HasName("PK__CodeSanP__152C7C5DA64542B8");

            entity.ToTable("CodeSanPham", tb => tb.HasTrigger("before_insert_CodeSanPham"));

            entity.HasOne(d => d.MaSanPhamNavigation).WithMany(p => p.CodeSanPhams)
                .HasForeignKey(d => d.MaSanPham)
                .HasConstraintName("FK__CodeSanPh__MaSan__49C3F6B7");
        });

        modelBuilder.Entity<DonDatHang>(entity =>
        {
            entity.HasKey(e => e.MaDonHang).HasName("PK__DonDatHa__129584ADC77C0569");

            entity.ToTable("DonDatHang", tb => tb.HasTrigger("before_insert_DonDatHang"));

            entity.Property(e => e.MaDonHang)
                .HasMaxLength(16)
                .IsUnicode(false);
            entity.Property(e => e.NgayDat)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ThanhToan)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.MaKhachHangNavigation).WithMany(p => p.DonDatHangs)
                .HasForeignKey(d => d.MaKhachHang)
                .HasConstraintName("FK__DonDatHan__MaKha__403A8C7D");

            entity.HasOne(d => d.MaSanPhamNavigation).WithMany(p => p.DonDatHangs)
                .HasForeignKey(d => d.MaSanPham)
                .HasConstraintName("FK__DonDatHan__MaSan__412EB0B6");
        });

        modelBuilder.Entity<KhachHang>(entity =>
        {
            entity.HasKey(e => e.MaKhachHang).HasName("PK__KhachHan__88D2F0E51516340D");

            entity.ToTable("KhachHang");

            entity.Property(e => e.DiaChi)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.SoDienThoai)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.TenKhachHang)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<NhanVien>(entity =>
        {
            entity.HasKey(e => e.MaNhanVien).HasName("PK__NhanVien__77B2CA470C5D05BA");

            entity.ToTable("NhanVien");

            entity.HasIndex(e => e.TaiKhoan, "UQ__NhanVien__D5B8C7F0B3C2EFF2").IsUnique();

            entity.Property(e => e.MatKhau)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TaiKhoan)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TenNhanVien)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<SanPham>(entity =>
        {
            entity.HasKey(e => e.MaSanPham).HasName("PK__SanPham__FAC7442D49EC1853");

            entity.ToTable("SanPham");

            entity.HasIndex(e => e.MaSoSanPham, "UQ__SanPham__F2321537A26A0091").IsUnique();

            entity.Property(e => e.Gia).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.MaSoSanPham)
                .HasMaxLength(7)
                .IsUnicode(false);
            entity.Property(e => e.MoTaSanPham).HasColumnType("text");
            entity.Property(e => e.TenSanPham)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<ThanhToan>(entity =>
        {
            entity.HasKey(e => e.MaThanhToan).HasName("PK__ThanhToa__D4B2584482EBD7F3");

            entity.ToTable("ThanhToan");

            entity.Property(e => e.MaDonHang)
                .HasMaxLength(16)
                .IsUnicode(false);
            entity.Property(e => e.PhuongThucThanhToan)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.ThoiGian)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.MaDonHangNavigation).WithMany(p => p.ThanhToans)
                .HasForeignKey(d => d.MaDonHang)
                .HasConstraintName("FK__ThanhToan__MaDon__44FF419A");

            entity.HasOne(d => d.MaKhachHangNavigation).WithMany(p => p.ThanhToans)
                .HasForeignKey(d => d.MaKhachHang)
                .HasConstraintName("FK__ThanhToan__MaKha__45F365D3");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
