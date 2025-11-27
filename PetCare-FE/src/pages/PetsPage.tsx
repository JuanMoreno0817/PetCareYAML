import React, { useEffect, useMemo, useState } from "react";
import { useNavigate } from "react-router-dom";
import { fetchPets, createPet, editPet, deletePet } from "../services/petService";
import type { Pet } from "../types/pet";

const generoLabel = (g?: number) => (g === 0 ? "Hembra" : g === 1 ? "Macho" : "-");
const tipoLabel = (t?: number) => (t === 0 ? "Perro" : t === 1 ? "Gato" : t === 2 ? "Otro" : "-");
const statusLabel = (s?: number) =>
    s === 1 ? "Disponible" : s === 2 ? "Adoptado" : s === 3 ? "Reservado" : "Inactivo";

const statusBadge = (s?: number) => {
    switch (s) {
        case 1:
            return "bg-green-100 text-green-700 ring-1 ring-green-200";
        case 2:
            return "bg-blue-100 text-blue-700 ring-1 ring-blue-200";
        case 3:
            return "bg-amber-100 text-amber-700 ring-1 ring-amber-200";
        default:
            return "bg-gray-100 text-gray-700 ring-1 ring-gray-200";
    }
};

const shimmer = "animate-pulse bg-gradient-to-r from-slate-100 via-slate-200 to-slate-100";

const PetsPage: React.FC = () => {
    const [pets, setPets] = useState<Pet[]>([]);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState<string | null>(null);
    const [formOpen, setFormOpen] = useState(false);
    const [form, setForm] = useState<Partial<Pet>>({});
    const [editingId, setEditingId] = useState<number | null>(null);
    const [query, setQuery] = useState("");
    const [onlyAvailable, setOnlyAvailable] = useState(false);
    const navigate = useNavigate();

    const loadPets = async () => {
        setLoading(true);
        setError(null);
        try {
            const data = await fetchPets();
            setPets(data);
        } catch (err: any) {
            setError(err.message || "Error inesperado");
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        loadPets();
    }, []);

    const filtered = useMemo(() => {
        const q = query.trim().toLowerCase();
        return pets.filter((p) => {
            const hayCoincidencia =
                !q ||
                [p.name, p.race, p.color, p.description]
                    .filter(Boolean)
                    .some((v) => String(v).toLowerCase().includes(q));
            const disponible = !onlyAvailable || p.status === 1;
            return hayCoincidencia && disponible;
        });
    }, [pets, query, onlyAvailable]);

    const handleInputChange = (
        e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement | HTMLSelectElement>
    ) => {
        const { name, value, type } = e.target as HTMLInputElement;
        setForm((prev) => ({
            ...prev,
            [name]: type === "number" || name === "age" || name === "weight" || name === "height" || name === "genero" || name === "status" || name === "tipo" || name === "idMedicalRecord"
                ? Number(value)
                : value,
        }));
    };

    const resetForm = () => {
        setForm({});
        setEditingId(null);
    };

    const openCreate = () => {
        resetForm();
        setFormOpen(true);
    };

    const openEdit = (pet: Pet) => {
        setForm(pet);
        setEditingId(pet.idPet);
        setFormOpen(true);
    };

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        setLoading(true);
        setError(null);
        try {
            if (editingId) {
                await editPet(editingId, form);
            } else {
                await createPet(form as Omit<Pet, "idPet">);
            }
            resetForm();
            setFormOpen(false);
            await loadPets();
        } catch (err: any) {
            setError(err.message || "Error al guardar");
        } finally {
            setLoading(false);
        }
    };

    const handleDelete = async (idPet: number) => {
        if (!window.confirm("¿Seguro que deseas eliminar esta mascota?")) return;
        setLoading(true);
        setError(null);
        try {
            await deletePet(idPet);
            await loadPets();
        } catch (err: any) {
            setError(err.message || "Error al eliminar");
        } finally {
            setLoading(false);
        }
    };

    return (
        <div className="min-h-screen flex flex-col">
            {/* Header */}
            <header className="py-4 shadow-md bg-white/40">
                <div className="max-w-6xl mx-auto flex justify-between items-center px-6 md:px-12">
                    <h1 className="text-3xl font-bold" style={{ color: "var(--color-primary)" }}>
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

            {/* Main Content */}
            <div className="flex-1 p-6 md:p-12">
                <section className="max-w-6xl mx-auto mb-10">
                    <div className="flex justify-between items-center mb-8">
                        <div>
                            <h1 className="text-3xl font-bold mb-2" style={{ color: "var(--color-primary)" }}>
                                Gestión de Mascotas
                            </h1>
                            <p style={{ color: "rgba(37,55,84,0.85)" }}>
                                Administra y busca mascotas disponibles para adopción
                            </p>
                        </div>
                        <button
                            onClick={openCreate}
                            className="px-6 py-3 rounded-2xl font-semibold shadow-md hover:scale-[1.02] transition-transform"
                            style={{
                                backgroundColor: "var(--color-accent)",
                                color: "var(--color-white)",
                                cursor: "pointer",
                            }}
                        >
                            Agregar Mascota
                        </button>
                    </div>

                    {/* Search and Filters */}
                    <div className="p-6 rounded-xl shadow-sm mb-8" style={{ backgroundColor: "var(--color-white)" }}>
                        <div className="grid grid-cols-1 md:grid-cols-3 gap-6">
                            <div className="md:col-span-2">
                                <input
                                    value={query}
                                    onChange={(e) => setQuery(e.target.value)}
                                    placeholder="Buscar por nombre, raza, color o descripción..."
                                    className="w-full p-3 rounded-lg border"
                                    style={{
                                        borderColor: "var(--color-soft)",
                                        color: "var(--color-primary)"
                                    }}
                                />
                            </div>
                            <div className="flex items-center">
                                <label className="flex items-center gap-3 text-sm" style={{ color: "var(--color-primary)" }}>
                                    <input
                                        type="checkbox"
                                        checked={onlyAvailable}
                                        onChange={(e) => setOnlyAvailable(e.target.checked)}
                                        className="w-4 h-4"
                                        style={{ accentColor: "var(--color-accent)" }}
                                    />
                                    Solo disponibles
                                </label>
                            </div>
                        </div>
                    </div>

                    {/* Error Message */}
                    {error && (
                        <div className="mb-6 p-4 rounded-lg" style={{
                            backgroundColor: "var(--color-white)",
                            border: "1px solid #fecaca",
                            color: "#dc2626"
                        }}>
                            {error}
                        </div>
                    )}

                    {/* Loading State */}
                    {loading && (
                        <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-6">
                            {Array.from({ length: 6 }).map((_, i) => (
                                <div key={i} className={`h-80 rounded-2xl ${shimmer}`}></div>
                            ))}
                        </div>
                    )}

                    {/* Empty State */}
                    {!loading && filtered.length === 0 && (
                        <div className="text-center py-12">
                            <p style={{ color: "rgba(37,55,84,0.85)" }}>No hay mascotas para mostrar.</p>
                        </div>
                    )}

                    {/* Pets Grid */}
                    {!loading && filtered.length > 0 && (
                        <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-6">
                            {filtered.map((pet) => (
                                <div
                                    key={pet.idPet}
                                    className="rounded-2xl shadow-sm hover:shadow-md transition-all duration-300 overflow-hidden"
                                    style={{ backgroundColor: "var(--color-white)" }}
                                >
                                    {/* Pet Image Placeholder */}
                                    <div
                                        className="h-48 w-full flex items-center justify-center"
                                        style={{ backgroundColor: "var(--color-soft)" }}
                                    >
                                        <p style={{ color: "var(--color-primary)" }}>
                                            {pet.tipo === 0 ? "🐶" : pet.tipo === 1 ? "🐱" : "🐾"}
                                        </p>
                                    </div>

                                    <div className="p-6">
                                        <div className="flex justify-between items-start mb-3">
                                            <h3 className="text-xl font-semibold" style={{ color: "var(--color-primary)" }}>
                                                {pet.name}
                                            </h3>
                                            <span className={`px-3 py-1 rounded-full text-sm font-medium ${statusBadge(pet.status)}`}>
                                                {statusLabel(pet.status)}
                                            </span>
                                        </div>

                                        <p className="text-sm mb-4" style={{ color: "rgba(37,55,84,0.85)" }}>
                                            {pet.description}
                                        </p>

                                        <div className="grid grid-cols-2 gap-3 text-sm mb-4">
                                            <div>
                                                <span style={{ color: "var(--color-primary)" }} className="font-medium">Raza:</span>
                                                <p style={{ color: "rgba(37,55,84,0.85)" }}>{pet.race}</p>
                                            </div>
                                            <div>
                                                <span style={{ color: "var(--color-primary)" }} className="font-medium">Color:</span>
                                                <p style={{ color: "rgba(37,55,84,0.85)" }}>{pet.color}</p>
                                            </div>
                                            <div>
                                                <span style={{ color: "var(--color-primary)" }} className="font-medium">Edad:</span>
                                                <p style={{ color: "rgba(37,55,84,0.85)" }}>{pet.age} años</p>
                                            </div>
                                            <div>
                                                <span style={{ color: "var(--color-primary)" }} className="font-medium">Peso:</span>
                                                <p style={{ color: "rgba(37,55,84,0.85)" }}>{pet.weight} kg</p>
                                            </div>
                                            <div>
                                                <span style={{ color: "var(--color-primary)" }} className="font-medium">Altura:</span>
                                                <p style={{ color: "rgba(37,55,84,0.85)" }}>{pet.height} cm</p>
                                            </div>
                                            <div>
                                                <span style={{ color: "var(--color-primary)" }} className="font-medium">Género:</span>
                                                <p style={{ color: "rgba(37,55,84,0.85)" }}>{generoLabel(pet.genero)}</p>
                                            </div>
                                            <div>
                                                <span style={{ color: "var(--color-primary)" }} className="font-medium">Tipo:</span>
                                                <p style={{ color: "rgba(37,55,84,0.85)" }}>{tipoLabel(pet.tipo)}</p>
                                            </div>
                                            <div>
                                                <span style={{ color: "var(--color-primary)" }} className="font-medium">Historia:</span>
                                                <p style={{ color: "rgba(37,55,84,0.85)" }}>#{pet.idMedicalRecord}</p>
                                            </div>
                                        </div>

                                        <div className="flex gap-3">
                                            <button
                                                onClick={() => openEdit(pet)}
                                                className="flex-1 px-4 py-2 rounded-lg font-medium hover:scale-105 transition-transform"
                                                style={{
                                                    border: "2px solid var(--color-primary)",
                                                    color: "var(--color-primary)",
                                                    backgroundColor: "transparent",
                                                    cursor: "pointer",
                                                }}
                                            >
                                                Editar
                                            </button>
                                            <button
                                                onClick={() => handleDelete(pet.idPet)}
                                                className="flex-1 px-4 py-2 rounded-lg font-semibold shadow hover:scale-105 transition-transform"
                                                style={{
                                                    backgroundColor: "#ef4444",
                                                    color: "var(--color-white)",
                                                    cursor: "pointer",
                                                }}
                                            >
                                                Eliminar
                                            </button>
                                        </div>
                                    </div>
                                </div>
                            ))}
                        </div>
                    )}
                </section>
            </div>

            {/* Modal Form */}
            {formOpen && (
                <div className="fixed inset-0 z-50 flex items-center justify-center p-4 bg-black/50">
                    <div className="w-full max-w-4xl rounded-2xl shadow-xl" style={{ backgroundColor: "var(--color-white)" }}>
                        <div className="p-6 border-b" style={{ borderColor: "var(--color-soft)" }}>
                            <h2 className="text-2xl font-semibold" style={{ color: "var(--color-primary)" }}>
                                {editingId ? "Actualizar Mascota" : "Agregar Mascota"}
                            </h2>
                            <p style={{ color: "rgba(37,55,84,0.85)" }}>Completa la información requerida</p>
                        </div>

                        <form onSubmit={handleSubmit} className="p-6">
                            <div className="grid grid-cols-1 md:grid-cols-2 gap-4 mb-6">
                                <input
                                    name="name"
                                    placeholder="Nombre"
                                    value={form.name || ""}
                                    onChange={handleInputChange}
                                    className="p-3 rounded-lg border"
                                    style={{
                                        borderColor: "var(--color-soft)",
                                        color: "var(--color-primary)"
                                    }}
                                    required
                                />
                                <input
                                    name="age"
                                    type="number"
                                    placeholder="Edad (años)"
                                    value={form.age ?? ""}
                                    onChange={handleInputChange}
                                    className="p-3 rounded-lg border"
                                    style={{
                                        borderColor: "var(--color-soft)",
                                        color: "var(--color-primary)"
                                    }}
                                    required
                                />
                                <input
                                    name="color"
                                    placeholder="Color"
                                    value={form.color || ""}
                                    onChange={handleInputChange}
                                    className="p-3 rounded-lg border"
                                    style={{
                                        borderColor: "var(--color-soft)",
                                        color: "var(--color-primary)"
                                    }}
                                    required
                                />
                                <input
                                    name="race"
                                    placeholder="Raza"
                                    value={form.race || ""}
                                    onChange={handleInputChange}
                                    className="p-3 rounded-lg border"
                                    style={{
                                        borderColor: "var(--color-soft)",
                                        color: "var(--color-primary)"
                                    }}
                                    required
                                />
                                <input
                                    name="weight"
                                    type="number"
                                    placeholder="Peso (kg)"
                                    value={form.weight ?? ""}
                                    onChange={handleInputChange}
                                    className="p-3 rounded-lg border"
                                    style={{
                                        borderColor: "var(--color-soft)",
                                        color: "var(--color-primary)"
                                    }}
                                    required
                                />
                                <input
                                    name="height"
                                    type="number"
                                    placeholder="Altura (cm)"
                                    value={form.height ?? ""}
                                    onChange={handleInputChange}
                                    className="p-3 rounded-lg border"
                                    style={{
                                        borderColor: "var(--color-soft)",
                                        color: "var(--color-primary)"
                                    }}
                                    required
                                />
                                <select
                                    name="genero"
                                    value={form.genero ?? ""}
                                    onChange={handleInputChange}
                                    className="p-3 rounded-lg border"
                                    style={{
                                        borderColor: "var(--color-soft)",
                                        color: "var(--color-primary)"
                                    }}
                                    required
                                >
                                    <option value="" disabled>Género</option>
                                    <option value={0}>Hembra</option>
                                    <option value={1}>Macho</option>
                                </select>
                                <select
                                    name="status"
                                    value={form.status ?? ""}
                                    onChange={handleInputChange}
                                    className="p-3 rounded-lg border"
                                    style={{
                                        borderColor: "var(--color-soft)",
                                        color: "var(--color-primary)"
                                    }}
                                    required
                                >
                                    <option value="" disabled>Estado</option>
                                    <option value={1}>Disponible</option>
                                    <option value={3}>Reservado</option>
                                    <option value={2}>Adoptado</option>
                                    <option value={0}>Inactivo</option>
                                </select>
                                <select
                                    name="tipo"
                                    value={form.tipo ?? ""}
                                    onChange={handleInputChange}
                                    className="p-3 rounded-lg border"
                                    style={{
                                        borderColor: "var(--color-soft)",
                                        color: "var(--color-primary)"
                                    }}
                                    required
                                >
                                    <option value="" disabled>Tipo</option>
                                    <option value={0}>Perro</option>
                                    <option value={1}>Gato</option>
                                    <option value={2}>Otro</option>
                                </select>
                                <input
                                    name="idMedicalRecord"
                                    type="number"
                                    placeholder="ID Historia Médica"
                                    value={form.idMedicalRecord ?? ""}
                                    onChange={handleInputChange}
                                    className="p-3 rounded-lg border"
                                    style={{
                                        borderColor: "var(--color-soft)",
                                        color: "var(--color-primary)"
                                    }}
                                    required
                                />
                                <textarea
                                    name="description"
                                    placeholder="Descripción"
                                    value={form.description || ""}
                                    onChange={handleInputChange}
                                    className="md:col-span-2 p-3 rounded-lg border min-h-24"
                                    style={{
                                        borderColor: "var(--color-soft)",
                                        color: "var(--color-primary)"
                                    }}
                                    required
                                />
                            </div>

                            <div className="flex justify-end gap-4">
                                <button
                                    type="button"
                                    onClick={() => {
                                        setFormOpen(false);
                                        setTimeout(() => resetForm(), 150);
                                    }}
                                    className="px-6 py-3 rounded-lg font-medium hover:scale-105 transition-transform"
                                    style={{
                                        border: "2px solid var(--color-primary)",
                                        color: "var(--color-primary)",
                                        backgroundColor: "transparent",
                                        cursor: "pointer",
                                    }}
                                >
                                    Cancelar
                                </button>
                                <button
                                    type="submit"
                                    disabled={loading}
                                    className="px-6 py-3 rounded-lg font-semibold shadow hover:scale-105 transition-transform disabled:opacity-50"
                                    style={{
                                        backgroundColor: "var(--color-accent)",
                                        color: "var(--color-white)",
                                        cursor: "pointer",
                                    }}
                                >
                                    {editingId ? "Actualizar" : "Agregar"}
                                </button>
                            </div>
                        </form>
                    </div>
                </div>
            )}

            {/* Footer */}
            <footer className="py-6 text-center" style={{
                backgroundColor: "var(--color-primary)",
                color: "var(--color-white)"
            }}>
                <p>&copy; 2025 PetCare. Todos los derechos reservados.</p>
                <p className="text-sm mt-2" style={{ color: "rgba(255,255,255,0.8)" }}>
                    Trabajo realizado por Juan Pablo Moreno - Zara Ramírez - Juan Diego Pabón
                </p>
            </footer>
        </div>
    );
};

export default PetsPage;