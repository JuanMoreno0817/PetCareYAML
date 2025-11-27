import React from "react";
import { useAuth0 } from "@auth0/auth0-react";

const Home: React.FC = () => {
  const { loginWithRedirect } = useAuth0();

  return (
    <div className="min-h-screen flex flex-col">
      <div
        className="min-h-screen flex flex-col"
        style={{ backgroundColor: "var(--color-soft)" }}
      >
        <header className="py-4 shadow-md bg-white/40">
          <div className="max-w-6xl mx-auto flex justify-between items-center px-6 md:px-12">
            <h1
              className="text-3xl font-bold"
              style={{ color: "var(--color-primary)", cursor: "pointer" }}
            >
              PetCare
            </h1>
            <button
              onClick={() => loginWithRedirect()}
              className="px-4 py-2 rounded-lg font-semibold shadow hover:scale-105 transition-transform"
              style={{
                backgroundColor: "var(--color-accent)",
                color: "var(--color-white)",
                cursor: "pointer",
              }}
            >
              Iniciar Sesión
            </button>
          </div>
        </header>
        <section className="max-w-6xl mx-auto grid grid-cols-1 md:grid-cols-2 gap-8 items-center mt-8">
          <div className="space-y-6">
            <h1
              className="text-4xl md:text-5xl font-bold leading-tight"
              style={{ color: "var(--color-primary)", cursor: "pointer" }}
            >
              PetCare
            </h1>
            <p
              className="text-lg md:text-xl max-w-xl"
              style={{ color: "rgba(37,55,84,0.9)" }}
            >
              Tu plataforma integral para el cuidado y adopción de mascotas
            </p>
            <button
              className="px-6 py-3 rounded-2xl font-semibold shadow-md hover:scale-[1.02] transition-transform"
              style={{
                backgroundColor: "var(--color-accent)",
                color: "var(--color-white)",
              }}
            >
              Explorar Mascotas
            </button>
          </div>

          <div className="flex justify-center">
            <div
              className="w-full max-w-md rounded-lg p-6 shadow-lg"
              style={{ backgroundColor: "var(--color-white)" }}
            >
              <div
                className="h-56 rounded-md flex items-center justify-center border-2 border-dashed"
                style={{ borderColor: "var(--color-primary)" }}
              >
                <p style={{ color: "var(--color-primary)" }}>
                  Imagen de mascotas felices
                </p>
              </div>
            </div>
          </div>
        </section>

        <section className="max-w-6xl mx-auto mt-12">
          <h2
            className="text-2xl font-semibold mb-6"
            style={{ color: "var(--color-primary)" }}
          >
            ¿Qué ofrece PetCare?
          </h2>

          <div className="grid grid-cols-1 md:grid-cols-3 gap-6">
            {[
              {
                title: "Adopción Responsable",
                desc: "Conectamos centros de rescate con familias amorosas para darle un hogar a cada mascota necesitada.",
              },
              {
                title: "Cuidado Integral",
                desc: "Ofrecemos herramientas para el seguimiento del bienestar y desarrollo de cada mascota.",
              },
              {
                title: "Registro Sanitario",
                desc: "Mantén un historial completo de vacunas, tratamientos y controles veterinarios.",
              },
            ].map((f) => (
              <div
                key={f.title}
                className="p-6 rounded-xl shadow-sm"
                style={{ backgroundColor: "var(--color-white)" }}
              >
                <div
                  className="h-36 rounded-md mb-4 flex items-center justify-center border"
                  style={{ borderColor: "var(--color-soft)" }}
                >
                  <p style={{ color: "var(--color-primary)" }}>Imagen</p>
                </div>
                <h3
                  className="text-lg font-semibold mb-2"
                  style={{ color: "var(--color-primary)" }}
                >
                  {f.title}
                </h3>
                <p
                  className="text-sm text-justify"
                  style={{ color: "rgba(37,55,84,0.85)" }}
                >
                  {f.desc}
                </p>
              </div>
            ))}
          </div>
        </section>

        <section className="max-w-6xl mx-auto mt-12">
          <h2
            className="text-2xl font-semibold mb-6"
            style={{ color: "var(--color-primary)" }}
          >
            ¿Cómo funciona?
          </h2>

          <div className="grid grid-cols-1 md:grid-cols-4 gap-4">
            {["Registro", "Gestión", "Seguimiento", "Conexión"].map((s, i) => (
              <div
                key={s}
                className="flex items-start gap-4 p-4 rounded-lg"
                style={{ backgroundColor: "var(--color-white)" }}
              >
                <div
                  className="flex-none w-10 h-10 rounded-full flex items-center justify-center font-bold"
                  style={{
                    backgroundColor: "var(--color-primary)",
                    color: "var(--color-white)",
                  }}
                >
                  {i + 1}
                </div>
                <div>
                  <h3
                    className="font-semibold"
                    style={{ color: "var(--color-primary)" }}
                  >
                    {s}
                  </h3>
                  <p
                    className="text-sm text-justify"
                    style={{ color: "rgba(37,55,84,0.85)" }}
                  >
                    {i === 0
                      ? "Los centros de rescate se registran en la plataforma"
                      : i === 1
                      ? "Administran el perfil de cada mascota disponible para adopción"
                      : i === 2
                      ? "Mantienen el registro sanitario actualizado de cada animal"
                      : "Conectan con posibles adoptantes responsables"}
                  </p>
                </div>
              </div>
            ))}
          </div>
        </section>

        <section className="max-w-6xl mx-auto mt-12 text-center">
          <h2
            className="text-2xl font-semibold mb-4"
            style={{ color: "var(--color-primary)" }}
          >
            ¿Listo para transformar el cuidado animal?
          </h2>
          <p className="mb-6" style={{ color: "rgba(37,55,84,0.85)" }}>
            Únete a PetCare y sé parte del cambio
          </p>
          <div className="flex flex-col md:flex-row gap-4 justify-center">
            <button
              className="px-5 py-3 rounded-xl font-semibold shadow"
              style={{
                backgroundColor: "var(--color-primary)",
                color: "var(--color-white)",
              }}
            >
              Registrar Centro
            </button>
            <button
              className="px-5 py-3 rounded-xl font-semibold shadow"
              style={{
                border: "2px solid var(--color-primary)",
                color: "var(--color-primary)",
                backgroundColor: "transparent",
              }}
            >
              Explorar Mascotas
            </button>
          </div>
        </section>

        <footer
          className="py-6 text-center mt-6"
          style={{
            backgroundColor: "var(--color-primary)",
            color: "var(--color-white)",
          }}
        >
          <p>&copy; 2025 PetCare. Todos los derechos reservados.</p>
        </footer>
      </div>
    </div>
  );
};

export default Home;
