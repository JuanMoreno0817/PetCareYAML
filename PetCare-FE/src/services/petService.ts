import { API_BASE_URL, AUTH_TOKEN } from "../config/api";
import type { Pet } from "../types/pet";

const getHeaders = () => ({
  Authorization: `Bearer ${AUTH_TOKEN}`,
  "Content-Type": "application/json",
});

export const fetchPets = async (): Promise<Pet[]> => {
  const res = await fetch(`${API_BASE_URL}/Pet/GetPets`, {
    headers: getHeaders(),
  });
  if (!res.ok) throw new Error("Error al obtener mascotas");
  return res.json();
};

export const createPet = async (pet: Omit<Pet, "idPet">): Promise<Pet> => {
  const res = await fetch(`${API_BASE_URL}/Pet/CreatePet`, {
    method: "POST",
    headers: getHeaders(),
    body: JSON.stringify(pet),
  });
  if (!res.ok) throw new Error("Error al crear mascota");
  return res.json();
};

export const editPet = async (id: number, pet: Partial<Pet>): Promise<void> => {
  const res = await fetch(`${API_BASE_URL}/Pet/EditPet/${id}`, {
    method: "PUT",
    headers: getHeaders(),
    body: JSON.stringify({ ...pet, idPet: id }),
  });
  if (!res.ok) throw new Error("Error al editar mascota");
};

export const deletePet = async (id: number): Promise<void> => {
  const res = await fetch(`${API_BASE_URL}/Pet/DeletePet/${id}`, {
    method: "DELETE",
    headers: getHeaders(),
  });
  if (!res.ok) throw new Error("Error al eliminar mascota");
};
