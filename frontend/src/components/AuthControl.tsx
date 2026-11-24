import { Link } from "react-router-dom";
import { useAuth } from "../auth/AuthContext";

export function AuthControl({ className = "" }: { className?: string }) {
  const { isAuthenticated, signOut } = useAuth();

  const authButtonClassName = `inline-flex h-9 items-center justify-center rounded-[6px] border border-border px-4 text-[14px] text-text transition-colors hover:bg-surface focus-visible:ring-2 focus-visible:ring-accent focus-visible:outline-none ${className}`;

  return (
    <>
      {isAuthenticated ? (
        <button className={authButtonClassName} type="button" onClick={signOut}>
          Sign Out
        </button>
      ) : (
        <Link to="/login" className={authButtonClassName}>
          Sign In
        </Link>
      )}
    </>
  );
}
