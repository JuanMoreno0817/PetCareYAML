import { useAuth0 } from "@auth0/auth0-react";
import { API_BASE_URL } from "../config/api";

interface UserData {
  name: string;
  email: string | undefined;
  authId: string;
  identification: number;
  typeOfDocument: string;
}

export const useCreateAdopter = () => {
  const { user, isAuthenticated } = useAuth0();

  const createAdopter = async () => {
    if (!isAuthenticated || !user) {
      throw new Error("Usuario no autenticado");
    }

    const authId = user.sub?.split("|")[1] || "";
    const randomNumber = Math.floor(Math.random() * 1000);

    try {
      const userData: UserData = {
        name: user.nickname || "",
        email: user.email,
        authId: authId,
        identification:
          (user as any).user_metadata?.DocumentNumber || randomNumber,
        typeOfDocument:
          (user as any).user_metadata?.DocumentType || "Cédula de ciudadania",
      };

      const response = await fetch(`${API_BASE_URL}/Adopter/CreateAdopter`, {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(userData),
      });

      if (!response.ok) {
        throw new Error(`Error: ${response.status} ${response.statusText}`);
      }

      const result = await response.json();
      console.log("Usuario insertado exitosamente:", result);
      return result;
    } catch (error) {
      console.error("Error llamando al endpoint:", error);
      throw error;
    }
  };

  return { createAdopter };
};
