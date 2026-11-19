import { apiFetch } from "./apiClient";

export async function login(email: string, password: string): Promise<string> {
  const res = await apiFetch("/auth/login", {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({ email, password }),
  });

  if (!res.ok) throw new Error("Invalid email or password");

  const data = await res.json();

  return data.accessToken;
}
