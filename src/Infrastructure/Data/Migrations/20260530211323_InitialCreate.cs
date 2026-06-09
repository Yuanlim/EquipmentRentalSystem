using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace RentalSystem.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    EmployeeStatus = table.Column<int>(type: "integer", nullable: true),
                    UserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: true),
                    SecurityStamp = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Audits",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Action = table.Column<string>(type: "text", nullable: false),
                    EntityName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    EntityId = table.Column<Guid>(type: "uuid", nullable: true),
                    OldValue = table.Column<string>(type: "text", nullable: true),
                    NewValue = table.Column<string>(type: "text", nullable: true),
                    PerformedByUserId = table.Column<string>(type: "text", nullable: true),
                    PerformedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CautionLevel = table.Column<string>(type: "text", nullable: false),
                    Route = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    HttpMethod = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Audits", x => x.Id);
                    table.CheckConstraint("CK_Audit_OldAndNewCantBeSameValue", " \"OldValue\" IS NULL OR \"NewValue\" IS NULL OR \"OldValue\" <> \"NewValue\" ");
                    table.CheckConstraint("CK_Audit_PerformedAtCantBeFuture", " \"PerformedAt\" <= CURRENT_TIMESTAMP ");
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleId = table.Column<string>(type: "text", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    ProviderKey = table.Column<string>(type: "text", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    RoleId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EquipmentCategories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    NormalizedName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    CreatedByUserId = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentCategories", x => x.Id);
                    table.CheckConstraint("CK_EquipmentCategory_NormalizedAndOriginalHasSameLength", " LENGTH(\"Name\") = LENGTH(\"NormalizedName\") ");
                    table.ForeignKey(
                        name: "FK_EquipmentCategories_AspNetUsers_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RentalOrders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: false),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: false),
                    CustomerId = table.Column<string>(type: "text", nullable: false),
                    CustomerRequestNote = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    RentalStatus = table.Column<string>(type: "text", nullable: false),
                    PickUpAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ReturnedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RentalOrders", x => x.Id);
                    table.CheckConstraint("CK_RentalOrder_PickUpAtBeforeReturnedAt", " \"PickUpAt\" IS NULL OR \"ReturnedAt\" IS NULL OR \"PickUpAt\" <= \"ReturnedAt\" ");
                    table.CheckConstraint("CK_RentalOrder_StartDateAfterToday", " \"StartDate\" >= CURRENT_DATE ");
                    table.CheckConstraint("CK_RentalOrder_StartDateBeforeEndDate", " \"StartDate\" <= \"EndDate\" ");
                    table.CheckConstraint("CompletedNeedPickUpAndReturn", " \"RentalStatus\" <> 'Completed' OR (\"PickUpAt\" IS NOT NULL AND \"ReturnedAt\" IS NOT NULL) ");
                    table.ForeignKey(
                        name: "FK_RentalOrders_AspNetUsers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EquipmentItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DepositFee = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    FeePerDay = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    LateFeePerDay = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    Description = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Condition = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    SerialNumber = table.Column<string>(type: "text", nullable: false),
                    ImagePath = table.Column<string>(type: "text", nullable: true),
                    AssetTag = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentItems", x => x.Id);
                    table.CheckConstraint("CK_EquipmentItems_DepositFeeCantBeNegativeOrZero", "\"DepositFee\" > 0");
                    table.CheckConstraint("CK_EquipmentItems_FeePerDayCantBeNegativeOrZero", "\"FeePerDay\" > 0");
                    table.CheckConstraint("CK_EquipmentItems_LateFeePerDayCantBeNegativeOrZero", "\"LateFeePerDay\" > 0");
                    table.ForeignKey(
                        name: "FK_EquipmentItems_EquipmentCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "EquipmentCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Inspections",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RentalOrderId = table.Column<Guid>(type: "uuid", nullable: false),
                    RentalOrderItemId = table.Column<Guid>(type: "uuid", nullable: false),
                    TechnicianId = table.Column<string>(type: "text", nullable: false),
                    InspectionAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Comments = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    DeterminedCondition = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inspections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Inspections_AspNetUsers_TechnicianId",
                        column: x => x.TechnicianId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Inspections_RentalOrders_RentalOrderId",
                        column: x => x.RentalOrderId,
                        principalTable: "RentalOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RentalOrderId = table.Column<Guid>(type: "uuid", nullable: false),
                    PaymentType = table.Column<string>(type: "text", nullable: false),
                    PaymentStatus = table.Column<string>(type: "text", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    PaidAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.Id);
                    table.CheckConstraint("CK_Payment_AmountCantBeNegative", " \"Amount\" >= 0 ");
                    table.CheckConstraint("CK_Payment_PaidAtCantBeBeforeCreatedAt", " \"PaidAt\" >= \"CreatedAt\" ");
                    table.ForeignKey(
                        name: "FK_Payments_RentalOrders_RentalOrderId",
                        column: x => x.RentalOrderId,
                        principalTable: "RentalOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RentalStatusHistories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RentalOrderId = table.Column<Guid>(type: "uuid", nullable: false),
                    ChangedByUserId = table.Column<string>(type: "text", nullable: true),
                    OldStatus = table.Column<string>(type: "text", nullable: true),
                    NewStatus = table.Column<string>(type: "text", nullable: false),
                    Reason = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RentalStatusHistories", x => x.Id);
                    table.CheckConstraint("CK_RentalOrderHistory_OldAndNewValueCantBeSame", " \"OldStatus\" IS NULL OR \"OldStatus\" <> \"NewStatus\" ");
                    table.ForeignKey(
                        name: "FK_RentalStatusHistories_AspNetUsers_ChangedByUserId",
                        column: x => x.ChangedByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RentalStatusHistories_RentalOrders_RentalOrderId",
                        column: x => x.RentalOrderId,
                        principalTable: "RentalOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RentalOrderItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RentalOrderId = table.Column<Guid>(type: "uuid", nullable: false),
                    EquipmentItemId = table.Column<Guid>(type: "uuid", nullable: false),
                    InspectionId = table.Column<Guid>(type: "uuid", nullable: false),
                    FeePerDayAtRental = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    DepositFeeAtRental = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    LateFeePerDayAtRental = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RentalOrderItems", x => x.Id);
                    table.CheckConstraint("CK_RentalOrderItems_DepositFeeAtRentalCantBeNegativeOrZero", "\"DepositFeeAtRental\" > 0");
                    table.CheckConstraint("CK_RentalOrderItems_FeePerDayAtRentalCantBeNegativeOrZero", "\"FeePerDayAtRental\" > 0");
                    table.CheckConstraint("CK_RentalOrderItems_LateFeePerDayAtRentalCantBeNegativeOrZero", "\"LateFeePerDayAtRental\" > 0");
                    table.ForeignKey(
                        name: "FK_RentalOrderItems_EquipmentItems_EquipmentItemId",
                        column: x => x.EquipmentItemId,
                        principalTable: "EquipmentItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RentalOrderItems_Inspections_InspectionId",
                        column: x => x.InspectionId,
                        principalTable: "Inspections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RentalOrderItems_RentalOrders_RentalOrderId",
                        column: x => x.RentalOrderId,
                        principalTable: "RentalOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Damages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RentalOrderId = table.Column<Guid>(type: "uuid", nullable: false),
                    RentalOrderItemId = table.Column<Guid>(type: "uuid", nullable: false),
                    Description = table.Column<string>(type: "character varying(2500)", maxLength: 2500, nullable: false),
                    Fee = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Fixed = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    EquipmentItemId = table.Column<Guid>(type: "uuid", nullable: false),
                    MaintenanceJobId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Damages", x => x.Id);
                    table.CheckConstraint("CK_Damage_FeeCantBeNegative", " \"Fee\" > 0 ");
                    table.ForeignKey(
                        name: "FK_Damages_EquipmentItems_EquipmentItemId",
                        column: x => x.EquipmentItemId,
                        principalTable: "EquipmentItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Damages_RentalOrderItems_RentalOrderItemId",
                        column: x => x.RentalOrderItemId,
                        principalTable: "RentalOrderItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Damages_RentalOrders_RentalOrderId",
                        column: x => x.RentalOrderId,
                        principalTable: "RentalOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MaintenanceJobs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TechnicianId = table.Column<string>(type: "text", nullable: true),
                    DamageId = table.Column<Guid>(type: "uuid", nullable: false),
                    MaintenanceStatus = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaintenanceJobs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MaintenanceJobs_AspNetUsers_TechnicianId",
                        column: x => x.TechnicianId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MaintenanceJobs_Damages_DamageId",
                        column: x => x.DamageId,
                        principalTable: "Damages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Damages_MaintenanceJobId",
                table: "Damages",
                column: "MaintenanceJobId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Damages_RentalOrderId",
                table: "Damages",
                column: "RentalOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Damages_RentalOrderItemId",
                table: "Damages",
                column: "RentalOrderItemId");

            migrationBuilder.CreateIndex(
                name: "UX_Damages_EquipmentItemId_CantAddNewWhileOldNotResolved",
                table: "Damages",
                column: "EquipmentItemId",
                unique: true,
                filter: " \"Fixed\" = false ");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentCategories_CreatedByUserId",
                table: "EquipmentCategories",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentCategories_Name",
                table: "EquipmentCategories",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentItems_AssetTag",
                table: "EquipmentItems",
                column: "AssetTag",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentItems_CategoryId",
                table: "EquipmentItems",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Inspections_RentalOrderId_RentalOrderItemId",
                table: "Inspections",
                columns: new[] { "RentalOrderId", "RentalOrderItemId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Inspections_TechnicianId",
                table: "Inspections",
                column: "TechnicianId");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceJobs_DamageId",
                table: "MaintenanceJobs",
                column: "DamageId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceJobs_TechnicianId",
                table: "MaintenanceJobs",
                column: "TechnicianId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_RentalOrderId",
                table: "Payments",
                column: "RentalOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_RentalOrderItems_EquipmentItemId_RentalOrderId",
                table: "RentalOrderItems",
                columns: new[] { "EquipmentItemId", "RentalOrderId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RentalOrderItems_InspectionId",
                table: "RentalOrderItems",
                column: "InspectionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RentalOrderItems_RentalOrderId",
                table: "RentalOrderItems",
                column: "RentalOrderId");

            migrationBuilder.CreateIndex(
                name: "UX_RentalOrders_CustomerId_CanOnlyHasOneUnfinishedOrder",
                table: "RentalOrders",
                column: "CustomerId",
                unique: true,
                filter: " \"RentalStatus\" IN ('Pending', 'PickedUp', 'Approved', 'Returned')");

            migrationBuilder.CreateIndex(
                name: "IX_RentalStatusHistories_ChangedByUserId",
                table: "RentalStatusHistories",
                column: "ChangedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_RentalStatusHistories_RentalOrderId",
                table: "RentalStatusHistories",
                column: "RentalOrderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Audits");

            migrationBuilder.DropTable(
                name: "MaintenanceJobs");

            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "RentalStatusHistories");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Damages");

            migrationBuilder.DropTable(
                name: "RentalOrderItems");

            migrationBuilder.DropTable(
                name: "EquipmentItems");

            migrationBuilder.DropTable(
                name: "Inspections");

            migrationBuilder.DropTable(
                name: "EquipmentCategories");

            migrationBuilder.DropTable(
                name: "RentalOrders");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}
