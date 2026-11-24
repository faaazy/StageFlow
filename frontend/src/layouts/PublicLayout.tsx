import { Link, Outlet } from "react-router-dom";
import { AuthControl } from "../components/AuthControl";
import { Brand } from "../components/Brand";

export function PublicLayout() {
  return (
    <div className="flex min-h-svh flex-col bg-bg text-text">
      <header className="sticky top-0 z-20 h-16 border-b border-border bg-bg">
        <div className="mx-auto flex h-full w-full max-w-[72rem] items-center px-4 sm:px-6 lg:px-10">
          <Link
            to="/"
            aria-label="StageFlow home"
            className="rounded-[6px] focus-visible:ring-2 focus-visible:ring-accent focus-visible:outline-none"
          >
            <Brand />
          </Link>

          <nav
            aria-label="Public navigation"
            className="ml-auto flex items-center gap-2 sm:gap-4"
          >
            <Link
              to="/organizer"
              className="rounded-[6px] text-[15px] text-muted transition-colors hover:text-text focus-visible:ring-2 focus-visible:ring-accent focus-visible:outline-none"
            >
              For organizers
            </Link>
            <AuthControl />
          </nav>
        </div>
      </header>

      <main className="flex-1">
        <Outlet />
      </main>

      <footer className="border-t border-border">
        <div className="mx-auto w-full max-w-[72rem] px-4 py-6 sm:px-6 lg:px-10">
          <p className="text-[13px] text-muted">
            StageFlow · Festival Operations Platform
          </p>
        </div>
      </footer>
    </div>
  );
}
