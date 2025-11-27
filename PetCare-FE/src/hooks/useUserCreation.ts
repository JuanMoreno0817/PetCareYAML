import { useAuth0 } from "@auth0/auth0-react";
import { createUser, fetchUsers } from "../services/userService";
import type { User } from "../types/user";

export const useUserCreation = () => {
  const { user, isAuthenticated, isLoading } = useAuth0();

  const createAdopterOnFirstLogin = async () => {
    if (!isAuthenticated || isLoading || !user) {
      return;
    }
    
    const authId = user.sub;

    const userCreatedKey = `adopter_created_${authId}`;
    if (localStorage.getItem(userCreatedKey)) {
      console.log("El adoptante ya ha sido creado. No se requiere acción.");
      return;
    }

    try {
      const existingUsers = await fetchUsers();
      const userExists = existingUsers.some(
        (u) => (u as any).authId === authId
      );

      if (userExists) {
        localStorage.setItem(userCreatedKey, "true");
        console.log("El adoptante ya existe en la base de datos.");
        return;
      }

      const newUser: Omit<User, "identification"> = {
        name: user.given_name || user.nickname || "N/A",
        lastname: user.family_name || "N/A",
        cellphone: "N/A",
        address: "N/A",
        email: user.email!,
        password: "Auth0_user_password",
        borndate: new Date().toISOString().split("T")[0],
        userType: 0,
        ocupation: "N/A",
        houseType: 2,
        moneyIncome: 0,
      };
      
      await createUser(newUser);
      
      localStorage.setItem(userCreatedKey, "true");
      console.log("Adoptante creado exitosamente.");
    } catch (error) {
      console.error("Error al gestionar la creación del adoptante:", error);
    }
  };

  return { createAdopterOnFirstLogin };
};