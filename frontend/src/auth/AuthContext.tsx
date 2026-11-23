import { createContext, useContext, useState, type ReactNode } from "react";
import { clearToken, getToken, setToken } from "./tokenStorage";

type AuthContextValue = {
  isAuthenticated: boolean;
  signIn: (token: string) => void;
  signOut: () => void;
};

const AuthContext = createContext<AuthContextValue | null>(null);

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

export function useAuth() {
  const ctx = useContext(AuthContext);

  if (!ctx) {
    throw new Error("useAuth must be used inside AuthProvider");
  }

  return ctx;
}
