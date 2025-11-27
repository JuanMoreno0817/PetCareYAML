import { API_BASE_URL, AUTH_TOKEN } from "../config/api";
import type { User } from "../types/user";

const getHeaders = () => ({
  Authorization: `Bearer ${AUTH_TOKEN}`,
  "Content-Type": "application/json",
});

export const fetchUsers = async (): Promise<User[]> => {
  const res = await fetch(`${API_BASE_URL}/Adopter/GetAdopters`, {
    headers: getHeaders(),
  });
  if (!res.ok) throw new Error("Error al obtener adoptantes");
  return res.json();
};

export const createUser = async (
  user: Omit<User, "identification">
): Promise<User> => {
  const res = await fetch(`${API_BASE_URL}/Adopter/CreateAdopter`, {
    method: "POST",
    headers: getHeaders(),
    body: JSON.stringify(user),
  });
  if (!res.ok) throw new Error("Error al crear adoptante");
  return res.json();
};

export const editUser = async (
  id: number,
  user: Partial<User>
): Promise<void> => {
  const res = await fetch(`${API_BASE_URL}/Adopter/EditAdopter/${id}`, {
    method: "PUT",
    headers: getHeaders(),
    body: JSON.stringify({ ...user, identification: id }),
  });
  if (!res.ok) throw new Error("Error al editar adoptante");
};

export const deleteUser = async (id: number): Promise<void> => {
  const res = await fetch(`${API_BASE_URL}/Adopter/DeleteAdopter/${id}`, {
    method: "DELETE",
    headers: getHeaders(),
  });
  if (!res.ok) throw new Error("Error al eliminar adoptante");
};
