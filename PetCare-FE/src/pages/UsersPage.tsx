import React, { useEffect, useMemo, useState } from "react";
import { useNavigate } from "react-router-dom";
import { fetchUsers, createUser, editUser, deleteUser } from "../services/userService";
import type { User } from "../types/user";

const userTypeLabel = (u?: number) => (u === 0 ? "Usuario" : u === 1 ? "Admin" : "-");
const houseTypeLabel = (h?: number) => (h === 0 ? "Casa" : h === 1 ? "Apartamento" : "Otro");

const UsersPage: React.FC = () => {
    const [users, setUsers] = useState<User[]>([]);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState<string | null>(null);
    const [formOpen, setFormOpen] = useState(false);
    const [form, setForm] = useState<Partial<User>>({});
    const [editingId, setEditingId] = useState<number | null>(null);
    const [query, setQuery] = useState("");
    const navigate = useNavigate();

    const loadUsers = async () => {
        setLoading(true);
        setError(null);
        try {
            const data = await fetchUsers();
            setUsers(data);
        } catch (err: any) {
            setError(err.message || "Error inesperado");
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        loadUsers();
    }, []);

    const filtered = useMemo(() => {
        const q = query.trim().toLowerCase();
        return users.filter((u) =>
            !q ||
            [u.name, u.lastname, u.email, u.cellphone, u.address, u.ocupation]
                .filter(Boolean)
                .some((v) => String(v).toLowerCase().includes(q))
        );
    }, [users, query]);

    const handleInputChange = (
        e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement | HTMLSelectElement>
    ) => {
        const { name, value, type } = e.target as HTMLInputElement;
        setForm((prev) => ({
            ...prev,
            [name]:
                type === "number" || name === "identification" || name === "userType" || name === "houseType" || name === "moneyIncome"
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

    const openEdit = (user: User) => {
        setForm(user);
        setEditingId(user.identification);
        setFormOpen(true);
    };

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        setLoading(true);
        setError(null);
        try {
            if (editingId) {
                await editUser(editingId, form);
            } else {
                await createUser(form as Omit<User, "identification">);
            }
            resetForm();
            setFormOpen(false);
            await loadUsers();
        } catch (err: any) {
            setError(err.message || "Error al guardar");
        } finally {
            setLoading(false);
        }
    };

    const handleDelete = async (id: number) => {
        if (!window.confirm("¿Seguro que deseas eliminar este adoptante?")) return;
        setLoading(true);
        setError(null);
        try {
            await deleteUser(id);
            await loadUsers();
        } catch (err: any) {
            setError(err.message || "Error al eliminar");
        } finally {
            setLoading(false);
        }
    };

    return (
        <div className="min-h-screen flex flex-col">
            {/* Navbar */}
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

            {/* Header */}
            <header className="max-w-6xl mx-auto flex flex-col md:flex-row md:justify-between md:items-center px-4 py-6 md:px-12 mb-4 gap-4">
                <div className="flex items-center gap-3 flex-1">
                    <div className="size-9 rounded-xl bg-[#d89c6c]/10 grid place-items-center">
                        <span className="text-[#d89c6c] text-lg">👤</span>
                    </div>
                    <div>
                        <h1 className="text-2xl font-bold text-left" style={{ color: "var(--color-primary)" }}>Gestión de adoptantes</h1>
                        <p className="text-xs text-left" style={{ color: "rgba(37,55,84,0.7)" }}>CRUD de adoptantes</p>
                    </div>
                </div>
                <div className="flex justify-end">
                    <button
                        onClick={openCreate}
                        className="rounded-xl font-semibold shadow hover:scale-[1.02] transition-transform"
                        style={{ backgroundColor: "var(--color-accent)", color: "var(--color-white)", padding: "0.75rem 1.5rem", cursor: "pointer" }}
                    >
                        Agregar adoptante
                    </button>
                </div>
            </header>

            {/* Toolbar */}
            <section className="max-w-6xl mx-auto px-4 py-2">
                <div className="grid grid-cols-1 md:grid-cols-2 gap-3">
                    <input
                        value={query}
                        onChange={(e) => setQuery(e.target.value)}
                        placeholder="Buscar por nombre, apellido, email, celular, dirección u ocupación..."
                        className="w-full rounded-xl border border-slate-200 px-4 py-2 text-sm focus:outline-none focus:ring-2 focus:ring-[#d89c6c]"
                    />
                    <button
                        onClick={() => setQuery("")}
                        className="rounded-xl border border-slate-200 px-3 text-sm"
                        style={{ color: "var(--color-primary)", backgroundColor: "var(--color-white)", cursor: "pointer" }}
                    >
                        Limpiar
                    </button>
                </div>
            </section>

            {/* Content */}
            <main className="max-w-6xl mx-auto px-4 pb-16 flex-1">
                {error && (
                    <div className="mb-4 rounded-xl border border-rose-200 bg-rose-50 px-4 py-3 text-rose-700">
                        {error}
                    </div>
                )}

                {loading && (
                    <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-4">
                        {Array.from({ length: 6 }).map((_, i) => (
                            <div key={i} className="h-48 rounded-2xl animate-pulse bg-gradient-to-r from-[#e3d8d4] via-[#d89c6c]/20 to-[#e3d8d4]"></div>
                        ))}
                    </div>
                )}

                {!loading && filtered.length === 0 && (
                    <div className="mt-10 text-center" style={{ color: "rgba(37,55,84,0.7)" }}>
                        <p className="text-sm">No hay adoptantes para mostrar.</p>
                    </div>
                )}

                {!loading && filtered.length > 0 && (
                    <div className="overflow-x-auto rounded-2xl shadow border border-slate-200 bg-white">
                        <table className="w-full text-xs md:text-sm">
                            <thead>
                                <tr className="bg-slate-100 text-slate-700 sticky top-0 z-10">
                                    <th className="px-3 py-2 font-semibold">ID</th>
                                    <th className="px-3 py-2 font-semibold">Nombre</th>
                                    <th className="px-3 py-2 font-semibold">Apellido</th>
                                    <th className="px-3 py-2 font-semibold">Celular</th>
                                    <th className="px-3 py-2 font-semibold">Dirección</th>
                                    <th className="px-3 py-2 font-semibold">Email</th>
                                    <th className="px-3 py-2 font-semibold">Nacimiento</th>
                                    <th className="px-3 py-2 font-semibold">Tipo</th>
                                    <th className="px-3 py-2 font-semibold">Ocupación</th>
                                    <th className="px-3 py-2 font-semibold">Vivienda</th>
                                    <th className="px-3 py-2 font-semibold">Ingresos</th>
                                    <th className="px-3 py-2 font-semibold">Acciones</th>
                                </tr>
                            </thead>
                            <tbody>
                                {filtered.map((user, idx) => (
                                    <tr
                                        key={user.identification}
                                        className={
                                            idx % 2 === 0
                                                ? "bg-white hover:bg-sky-50 transition"
                                                : "bg-slate-50 hover:bg-sky-50 transition"
                                        }
                                    >
                                        <td className="px-3 py-2 font-mono text-xs text-sky-700">{user.identification}</td>
                                        <td className="px-3 py-2 font-medium text-slate-800">{user.name}</td>
                                        <td className="px-3 py-2">{user.lastname}</td>
                                        <td className="px-3 py-2">{user.cellphone}</td>
                                        <td className="px-3 py-2">{user.address}</td>
                                        <td className="px-3 py-2">
                                            <span className="inline-block rounded bg-sky-100 text-sky-700 px-2 py-0.5 text-xs font-mono">
                                                {user.email}
                                            </span>
                                        </td>
                                        <td className="px-3 py-2 text-xs text-slate-600">{user.borndate?.split("T")[0]}</td>
                                        <td className="px-3 py-2">
                                            <span className={`inline-block rounded-full px-2 py-0.5 text-xs font-semibold ${user.userType === 1 ? "bg-amber-100 text-amber-700" : "bg-sky-100 text-sky-700"}`}>
                                                {userTypeLabel(user.userType)}
                                            </span>
                                        </td>
                                        <td className="px-3 py-2">{user.ocupation}</td>
                                        <td className="px-3 py-2">
                                            <span className={`inline-block rounded-full px-2 py-0.5 text-xs font-semibold ${user.houseType === 1 ? "bg-emerald-100 text-emerald-700" : user.houseType === 0 ? "bg-sky-100 text-sky-700" : "bg-slate-200 text-slate-700"}`}>
                                                {houseTypeLabel(user.houseType)}
                                            </span>
                                        </td>
                                        <td className="px-3 py-2 text-right font-mono text-xs text-slate-700">${user.moneyIncome}</td>
                                        <td className="px-3 py-2">
                                            <div className="flex gap-1">
                                                <button
                                                    onClick={() => openEdit(user)}
                                                    className="rounded-lg border border-slate-200 px-3 py-1.5 text-xs text-sky-700 hover:bg-sky-50 transition cursor-pointer"
                                                    title="Editar adoptante"
                                                >
                                                    Editar
                                                </button>
                                                <button
                                                    onClick={() => handleDelete(user.identification)}
                                                    className="rounded-lg px-3 py-1.5 text-xs bg-rose-500/80 text-white hover:bg-rose-600 transition cursor-pointer"
                                                    title="Eliminar adoptante"
                                                >
                                                    Eliminar
                                                </button>
                                            </div>
                                        </td>
                                    </tr>
                                ))}
                            </tbody>
                        </table>
                    </div>
                )}
            </main>

            {/* Modal Form */}
            {formOpen && (
                <div className="fixed inset-0 z-20 grid place-items-center bg-black/30 p-4" role="dialog" aria-modal>
                    <div className="w-full max-w-2xl rounded-2xl shadow-xl" style={{ backgroundColor: "var(--color-white)" }}>
                        <div className="flex items-center justify-between border-b px-5 py-4">
                            <div>
                                <h2 className="text-lg font-semibold" style={{ color: "var(--color-primary)" }}>
                                    {editingId ? "Actualizar adoptante" : "Agregar adoptante"}
                                </h2>
                                <p className="text-xs" style={{ color: "rgba(37,55,84,0.7)" }}>Completa la información requerida</p>
                            </div>
                            <button
                                className="rounded-lg border border-slate-200 px-3 py-1 text-sm"
                                style={{ color: "var(--color-primary)", cursor: "pointer", backgroundColor: "var(--color-soft)" }}
                                onClick={() => {
                                    setFormOpen(false);
                                    setTimeout(() => resetForm(), 150);
                                }}
                            >
                                Cerrar
                            </button>
                        </div>

                        <form onSubmit={handleSubmit} className="px-5 pb-5 pt-4">
                            <div className="grid grid-cols-1 md:grid-cols-2 gap-3">
                                <input
                                    name="identification"
                                    type="number"
                                    placeholder="Identificación"
                                    value={form.identification ?? ""}
                                    onChange={handleInputChange}
                                    className="rounded-xl border border-slate-200 px-3 py-2 text-sm focus:outline-none focus:ring-2 focus:ring-[#d89c6c]"
                                    required
                                    disabled={!!editingId}
                                />
                                <input
                                    name="name"
                                    placeholder="Nombre"
                                    value={form.name || ""}
                                    onChange={handleInputChange}
                                    className="rounded-xl border border-slate-200 px-3 py-2 text-sm focus:outline-none focus:ring-2 focus:ring-[#d89c6c]"
                                    required
                                />
                                <input
                                    name="lastname"
                                    placeholder="Apellido"
                                    value={form.lastname || ""}
                                    onChange={handleInputChange}
                                    className="rounded-xl border border-slate-200 px-3 py-2 text-sm focus:outline-none focus:ring-2 focus:ring-[#d89c6c]"
                                    required
                                />
                                <input
                                    name="cellphone"
                                    placeholder="Celular"
                                    value={form.cellphone || ""}
                                    onChange={handleInputChange}
                                    className="rounded-xl border border-slate-200 px-3 py-2 text-sm focus:outline-none focus:ring-2 focus:ring-[#d89c6c]"
                                    required
                                />
                                <input
                                    name="address"
                                    placeholder="Dirección"
                                    value={form.address || ""}
                                    onChange={handleInputChange}
                                    className="rounded-xl border border-slate-200 px-3 py-2 text-sm focus:outline-none focus:ring-2 focus:ring-[#d89c6c]"
                                    required
                                />
                                <input
                                    name="email"
                                    type="email"
                                    placeholder="Email"
                                    value={form.email || ""}
                                    onChange={handleInputChange}
                                    className="rounded-xl border border-slate-200 px-3 py-2 text-sm focus:outline-none focus:ring-2 focus:ring-[#d89c6c]"
                                    required
                                />
                                <input
                                    name="password"
                                    type="password"
                                    placeholder="Contraseña"
                                    value={form.password || ""}
                                    onChange={handleInputChange}
                                    className="rounded-xl border border-slate-200 px-3 py-2 text-sm focus:outline-none focus:ring-2 focus:ring-[#d89c6c]"
                                    required={!editingId}
                                />
                                <input
                                    name="borndate"
                                    type="date"
                                    placeholder="Fecha de nacimiento"
                                    value={form.borndate ? form.borndate.split("T")[0] : ""}
                                    onChange={handleInputChange}
                                    className="rounded-xl border border-slate-200 px-3 py-2 text-sm focus:outline-none focus:ring-2 focus:ring-[#d89c6c]"
                                    required
                                />
                                <select
                                    name="userType"
                                    value={form.userType ?? ""}
                                    onChange={handleInputChange}
                                    className="rounded-xl border border-slate-200 px-3 py-2 text-sm focus:outline-none focus:ring-2 focus:ring-[#d89c6c]"
                                    required
                                >
                                    <option value="" disabled>
                                        Tipo de adoptante
                                    </option>
                                    <option value={0}>Usuario</option>
                                    <option value={1}>Admin</option>
                                </select>
                                <input
                                    name="ocupation"
                                    placeholder="Ocupación"
                                    value={form.ocupation || ""}
                                    onChange={handleInputChange}
                                    className="rounded-xl border border-slate-200 px-3 py-2 text-sm focus:outline-none focus:ring-2 focus:ring-[#d89c6c]"
                                    required
                                />
                                <select
                                    name="houseType"
                                    value={form.houseType ?? ""}
                                    onChange={handleInputChange}
                                    className="rounded-xl border border-slate-200 px-3 py-2 text-sm focus:outline-none focus:ring-2 focus:ring-[#d89c6c]"
                                    required
                                >
                                    <option value="" disabled>
                                        Tipo de vivienda
                                    </option>
                                    <option value={0}>Casa</option>
                                    <option value={1}>Apartamento</option>
                                    <option value={2}>Otro</option>
                                </select>
                                <input
                                    name="moneyIncome"
                                    type="number"
                                    placeholder="Ingresos"
                                    value={form.moneyIncome ?? ""}
                                    onChange={handleInputChange}
                                    className="rounded-xl border border-slate-200 px-3 py-2 text-sm focus:outline-none focus:ring-2 focus:ring-[#d89c6c]"
                                    required
                                />
                            </div>

                            <div className="mt-5 flex items-center justify-end gap-2">
                                {editingId && (
                                    <button
                                        type="button"
                                        onClick={resetForm}
                                        className="rounded-xl border border-slate-200 px-4 py-2 text-sm"
                                        style={{ color: "var(--color-primary)", cursor: "pointer", backgroundColor: "var(--color-soft)" }}
                                    >
                                        Limpiar cambios
                                    </button>
                                )}
                                <button
                                    type="button"
                                    onClick={() => {
                                        setFormOpen(false);
                                        setTimeout(() => resetForm(), 150);
                                    }}
                                    className="rounded-xl px-4 py-2 text-sm"
                                    style={{ color: "var(--color-primary)", cursor: "pointer", backgroundColor: "var(--color-soft)" }}
                                >
                                    Cancelar
                                </button>
                                <button
                                    type="submit"
                                    disabled={loading}
                                    className="rounded-xl font-semibold shadow"
                                    style={{ backgroundColor: "var(--color-accent)", cursor: "pointer", color: "var(--color-white)", padding: "0.75rem 1.5rem", opacity: loading ? 0.6 : 1 }}
                                >
                                    {editingId ? "Actualizar" : "Agregar"}
                                </button>
                            </div>
                        </form>
                    </div>
                </div>
            )}

            {/* Footer */}
            <footer className="py-6 text-center mt-6" style={{ backgroundColor: "var(--color-primary)", color: "var(--color-white)" }}>
                <p>
                    Trabajo realizado por Juan Pablo Moreno-Zara Ramírez -Juan Diego Pabón
                </p>
            </footer>
        </div>
    );
};

export default UsersPage;