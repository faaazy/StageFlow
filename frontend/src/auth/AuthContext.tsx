import { useState, type ReactNode } from "react";
import { clearToken, getToken, setToken } from "./tokenStorage";
import { AuthContext } from "./useAuth";

export function AuthProvider({ children }: { children: ReactNode }) {
  const [isAuthenticated, setIsAuthenticated] = useState(
    () => getToken() !== null,
  );

  function signIn(token: string) {
    setToken(token);

    setIsAuthenticated(true);
  }

  function signOut() {
    clearToken();

    setIsAuthenticated(false);
  }

  return (
    <AuthContext.Provider value={{ isAuthenticated, signIn, signOut }}>
      {children}
    </AuthContext.Provider>
  );
}
