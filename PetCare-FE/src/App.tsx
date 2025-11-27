import { useAuth0 } from "@auth0/auth0-react";
import { Routes, Route, Navigate } from "react-router-dom";
import { useEffect } from "react";
import Home from "./pages/Home";
import Dashboard from "./pages/Dashboard";
import PetsPage from "./pages/PetsPage";
import UsersPage from "./pages/UsersPage";
import { useUserCreation } from "./hooks/useUserCreation";

function App() {
  const { isAuthenticated, isLoading } = useAuth0();
  const { createAdopterOnFirstLogin } = useUserCreation();

  useEffect(() => {
    createAdopterOnFirstLogin();
  }, [isAuthenticated, isLoading, createAdopterOnFirstLogin]);

  if (isLoading) return <div className="text-center p-10">Cargando...</div>;

  return (
    <Routes>
      <Route path="/" element={<Home />} />
      <Route
        path="/dashboard"
        element={isAuthenticated ? <Dashboard /> : <Navigate to="/" />}
      />
      <Route
        path="/pets"
        element={isAuthenticated ? <PetsPage /> : <Navigate to="/" />}
      />
      <Route
        path="/users"
        element={isAuthenticated ? <UsersPage /> : <Navigate to="/" />}
      />
      <Route path="*" element={<Navigate to="/" />} />
    </Routes>
  );
}

export default App;