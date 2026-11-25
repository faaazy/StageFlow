import { Link, Outlet } from "react-router-dom";
import { AuthControl } from "../components/AuthControl";
import { Brand } from "../components/Brand";

export function PublicLayout() {
  return (
    <div className="poster-background flex min-h-svh flex-col bg-bg text-text">
      <div aria-hidden="true" className="poster-background-grid">
        <svg
          aria-hidden="true"
          className="size-full"
          xmlns="http://www.w3.org/2000/svg"
        >
          <defs>
            <pattern
              id="stageflow-registration"
              width="112"
              height="112"
              patternUnits="userSpaceOnUse"
            >
              <path
                d="M56 52v8M52 56h8"
                fill="none"
                stroke="currentColor"
                strokeWidth="1"
              />
            </pattern>
          </defs>
          <rect
            width="100%"
            height="100%"
            fill="url(#stageflow-registration)"
          />
        </svg>
      </div>

      <header className="sticky top-0 z-20 border-b border-border bg-bg">
        <div className="mx-auto flex h-14 w-full max-w-[72rem] items-center sm:h-16">
          <div className="flex h-full items-center px-2 sm:px-4">
            <Link
              to="/"
              aria-label="StageFlow home"
              className="rounded-[2px] focus-visible:ring-2 focus-visible:ring-accent focus-visible:outline-none"
            >
              <Brand />
            </Link>
          </div>

          <div className="ml-auto flex h-full items-stretch">
            <div className="flex h-full items-center border-l border-border px-2 sm:px-4">
              <Link
                to="/organizer"
                className="rounded-[2px] font-mono text-[13px] uppercase tracking-wider text-muted transition-colors hover:text-text focus-visible:ring-2 focus-visible:ring-accent focus-visible:outline-none"
              >
                <span className="sm:hidden">Organizers</span>
                <span className="hidden sm:inline">For organizers</span>
              </Link>
            </div>
            <div className="flex h-full items-center border-l border-border px-2 sm:px-4">
              <AuthControl />
            </div>
          </div>
        </div>
      </header>

      <main className="relative z-10 flex-1">
        <Outlet />
      </main>

      <footer className="relative z-10 border-t border-border">
        <div className="mx-auto w-full max-w-[72rem] px-4 py-6 sm:px-6 lg:px-10">
          <p className="font-mono text-[12px] uppercase tracking-wider text-muted">
            StageFlow · Festival Operations Platform
          </p>
        </div>
      </footer>
    </div>
  );
}
