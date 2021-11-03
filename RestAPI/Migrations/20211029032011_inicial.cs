using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RestAPI.Migrations
{
    public partial class inicial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Departamentos",
                columns: table => new
                {
                    IdDepartamento = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    NombreDepartamento = table.Column<string>(type: "varChar(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departamentos", x => x.IdDepartamento);
                });

            migrationBuilder.CreateTable(
                name: "Preguntas",
                columns: table => new
                {
                    IdPregunta = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Descripcion = table.Column<string>(type: "varChar(60)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Preguntas", x => x.IdPregunta);
                });

            migrationBuilder.CreateTable(
                name: "Respuestas",
                columns: table => new
                {
                    IdRespuesta = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Descripcion = table.Column<string>(type: "varChar(20)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Respuestas", x => x.IdRespuesta);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    IdUsuario = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    NombreUsuario = table.Column<string>(type: "varchar(30)", nullable: false),
                    Password = table.Column<string>(type: "varchar(30)", nullable: false),
                    Rol = table.Column<string>(type: "varchar(20)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.IdUsuario);
                });

            migrationBuilder.CreateTable(
                name: "Auditorias",
                columns: table => new
                {
                    IdAuditoria = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Descripcion = table.Column<string>(type: "varChar(50)", nullable: false),
                    Estado = table.Column<string>(type: "varChar(50)", nullable: false),
                    IdDepartamento_Auditoria = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Auditorias", x => x.IdAuditoria);
                    table.ForeignKey(
                        name: "FK_Auditorias_Departamentos_IdDepartamento_Auditoria",
                        column: x => x.IdDepartamento_Auditoria,
                        principalTable: "Departamentos",
                        principalColumn: "IdDepartamento",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Pregunta_Respuesta",
                columns: table => new
                {
                    IdPreguntaRespuesta = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IdPregunta_PreguntaRespuesta = table.Column<int>(nullable: false),
                    IdRespuesta_PreguntaRespuesta = table.Column<int>(nullable: false),
                    IdAuditoria_PreguntaRespuesta = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pregunta_Respuesta", x => x.IdPreguntaRespuesta);
                    table.ForeignKey(
                        name: "FK_Pregunta_Respuesta_Auditorias_IdAuditoria_PreguntaRespuesta",
                        column: x => x.IdAuditoria_PreguntaRespuesta,
                        principalTable: "Auditorias",
                        principalColumn: "IdAuditoria",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Pregunta_Respuesta_Preguntas_IdPregunta_PreguntaRespuesta",
                        column: x => x.IdPregunta_PreguntaRespuesta,
                        principalTable: "Preguntas",
                        principalColumn: "IdPregunta",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Pregunta_Respuesta_Respuestas_IdRespuesta_PreguntaRespuesta",
                        column: x => x.IdRespuesta_PreguntaRespuesta,
                        principalTable: "Respuestas",
                        principalColumn: "IdRespuesta",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Auditorias_IdDepartamento_Auditoria",
                table: "Auditorias",
                column: "IdDepartamento_Auditoria");

            migrationBuilder.CreateIndex(
                name: "IX_Pregunta_Respuesta_IdAuditoria_PreguntaRespuesta",
                table: "Pregunta_Respuesta",
                column: "IdAuditoria_PreguntaRespuesta");

            migrationBuilder.CreateIndex(
                name: "IX_Pregunta_Respuesta_IdPregunta_PreguntaRespuesta",
                table: "Pregunta_Respuesta",
                column: "IdPregunta_PreguntaRespuesta");

            migrationBuilder.CreateIndex(
                name: "IX_Pregunta_Respuesta_IdRespuesta_PreguntaRespuesta",
                table: "Pregunta_Respuesta",
                column: "IdRespuesta_PreguntaRespuesta");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Pregunta_Respuesta");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "Auditorias");

            migrationBuilder.DropTable(
                name: "Preguntas");

            migrationBuilder.DropTable(
                name: "Respuestas");

            migrationBuilder.DropTable(
                name: "Departamentos");
        }
    }
}
