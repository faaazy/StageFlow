import { createContext, useContext } from "react";

export type AuthContextValue = {
  isAuthenticated: boolean;
  signIn: (token: string) => void;
  signOut: () => void;
};

export const AuthContext = createContext<AuthContextValue | null>(null);

export function useAuth() {
  const ctx = useContext(AuthContext);

  if (!ctx) {
    throw new Error("useAuth must be used inside AuthProvider");
  }

  return ctx;
}
