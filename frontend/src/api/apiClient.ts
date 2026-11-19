import { getToken } from "../auth/tokenStorage";

export async function apiFetch(
  path: string,
  options: RequestInit = {},
): Promise<Response> {
  const finalPath = `http://localhost:5178${path}`;

  console.log("API REQUEST:", finalPath);

  const token = getToken();
  const headers = new Headers(options.headers);

  if (token) {
    headers.set("Authorization", `Bearer ${token}`);
  }

  return fetch(finalPath, { ...options, headers });
}
