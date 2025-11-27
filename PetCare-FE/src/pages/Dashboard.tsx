import React from "react";
import { useNavigate } from "react-router-dom";
import { useAuth0 } from "@auth0/auth0-react";

const Dashboard: React.FC = () => {
  const { logout } = useAuth0();

  const navigate = useNavigate();
  
  return (
    <div className="min-h-screen flex flex-col">
      <header className="py-4 shadow-md bg-white/40">
        <div className="max-w-6xl mx-auto flex justify-between items-center px-6 md:px-12">
          <h1
            className="text-3xl font-bold"
            style={{ color: "var(--color-primary)" }}
          >
            PetCare
          </h1>
          <div className="flex gap-4">
            <button
              onClick={() => navigate("/dashboard")}
              className="px-4 py-2 rounded-lg font-medium hover:scale-105 transition-transform"
              style={{
                border: "2px solid var(--color-primary)",
                color: "var(--color-primary)",
                backgroundColor: "transparent",
                cursor: "pointer",
              }}
            >
              Dashboard
            </button>
            <button
              onClick={() => navigate("/pets")}
              className="px-4 py-2 rounded-lg font-semibold shadow hover:scale-105 transition-transform"
              style={{
                backgroundColor: "var(--color-accent)",
                color: "var(--color-white)",
                cursor: "pointer",
              }}
            >
              Mascotas
            </button>
            <button
              onClick={() => navigate("/users")}
              className="px-4 py-2 rounded-lg font-semibold shadow hover:scale-105 transition-transform"
              style={{
                backgroundColor: "var(--color-accent)",
                color: "var(--color-white)",
                cursor: "pointer",
              }}
            >
              Adoptantes
            </button>
          </div>
        </div>
      </header>
      <div
        className="flex-1 p-6 md:p-12"
        style={{ backgroundColor: "var(--color-soft)" }}
      >
        <header className="max-w-6xl mx-auto flex justify-between items-center mb-10">
          <h1
            className="text-3xl font-bold"
            style={{ color: "var(--color-primary)" }}
          >
            Bienvenido a PetCare
          </h1>
          <button
            onClick={() =>
              logout({ logoutParams: { returnTo: window.location.origin } })
            }
            className="px-4 py-2 rounded-lg font-semibold shadow"
            style={{
              backgroundColor: "var(--color-accent)",
              color: "var(--color-white)",
              cursor: "pointer",
            }}
          >
            Cerrar Sesión
          </button>
        </header>

        <section className="max-w-6xl mx-auto grid grid-cols-1 md:grid-cols-3 gap-6">
          <div
            className="p-6 rounded-xl shadow-sm"
            style={{ backgroundColor: "var(--color-white)" }}
          >
            <h2
              className="text-lg font-semibold mb-2"
              style={{ color: "var(--color-primary)" }}
            >
              Explorar Mascotas
            </h2>
            <p className="text-sm" style={{ color: "rgba(37,55,84,0.85)" }}>
              Administra la información y el cuidado de tus mascotas
              registradas.
            </p>
            <button
              className="mt-4 px-4 py-2 rounded-lg font-medium shadow"
              style={{
                backgroundColor: "var(--color-primary)",
                color: "var(--color-white)",
                cursor: "pointer",
              }}
              onClick={() => navigate("/pets")}
            >
              Ver mascotas
            </button>
          </div>

          <div
            className="p-6 rounded-xl shadow-sm"
            style={{ backgroundColor: "var(--color-white)" }}
          >
            <h2
              className="text-lg font-semibold mb-2"
              style={{ color: "var(--color-primary)" }}
            >
              Explorar adoptantes
            </h2>
            <p className="text-sm" style={{ color: "rgba(37,55,84,0.85)" }}>
              Conoce las mascotas disponibles en centros de rescate.
            </p>
            <button
              className="mt-4 px-4 py-2 rounded-lg font-medium shadow"
              style={{
                backgroundColor: "var(--color-primary)",
                color: "var(--color-white)",
                cursor: "pointer",
              }}
              onClick={() => navigate("/users")}
            >
              Ver adoptantes
            </button>
          </div>
        </section>
      </div>

      <footer
        className="py-6 text-center"
        style={{
          backgroundColor: "var(--color-primary)",
          color: "var(--color-white)",
        }}
      >
        <p>&copy; 2025 PetCare. Todos los derechos reservados.</p>
      </footer>
    </div>
  );
};

export default Dashboard;