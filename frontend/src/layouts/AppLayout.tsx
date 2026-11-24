import { NavLink, Outlet } from "react-router-dom";
import { useState } from "react";
import {
  Clock,
  Flag,
  LayoutDashboard,
  Layers,
  type LucideIcon,
  Mic,
  Moon,
  ShoppingCart,
  Sun,
  Ticket,
} from "lucide-react";
import { Brand } from "../components/Brand";
import { AuthControl } from "../components/AuthControl";

const navIconProps = {
  "aria-hidden": true,
  strokeWidth: 1.5,
  className: "size-[18px] shrink-0",
};

const themeStorageKey = "stageflow-theme";

function ThemeToggle({
  isDark,
  onToggle,
}: {
  isDark: boolean;
  onToggle: () => void;
}) {
  const label = isDark ? "Switch to light theme" : "Switch to dark theme";

  return (
    <button
      type="button"
      onClick={onToggle}
      aria-label={label}
      title={label}
      aria-pressed={isDark}
      className="grid size-8 shrink-0 place-items-center rounded-[6px] border border-border text-muted transition-colors hover:text-text focus-visible:ring-2 focus-visible:ring-accent focus-visible:outline-none"
    >
      {isDark ? <Sun {...navIconProps} /> : <Moon {...navIconProps} />}
    </button>
  );
}

const navItems: { label: string; to?: string; icon: LucideIcon }[] = [
  { label: "Dashboard", icon: LayoutDashboard },
  { label: "Festivals", icon: Flag },
  { label: "Schedule", to: "schedule", icon: Clock },
  { label: "Artists", icon: Mic },
  { label: "Stages", icon: Layers },
  { label: "Tickets", icon: Ticket },
  { label: "Orders", icon: ShoppingCart },
];

function Sidebar({
  isDark,
  onToggleTheme,
}: {
  isDark: boolean;
  onToggleTheme: () => void;
}) {
  return (
    <aside className="sticky top-0 hidden h-svh w-56 shrink-0 flex-col border-r border-border bg-bg lg:flex">
      <div className="flex items-center px-6 pt-6 pb-6">
        <Brand />
      </div>

      <nav aria-label="Main" className="flex-1 overflow-y-auto py-2">
        <p className="px-6 pb-2 text-xs font-medium tracking-wider text-muted uppercase">
          Operations
        </p>
        <ul>
          {navItems.map((item) => (
            <li key={item.label}>
              {item.to ? (
                <NavLink
                  to={item.to}
                  className={({ isActive }) =>
                    `relative flex h-10 items-center gap-[10px] pl-[14px] pr-4 text-[15px] transition-colors ${
                      isActive
                        ? "bg-surface font-medium text-text"
                        : "text-muted hover:text-text"
                    }`
                  }
                >
                  {({ isActive }) => (
                    <>
                      {isActive && (
                        <span
                          aria-hidden="true"
                          className="absolute inset-y-0 left-0 w-[2px] bg-accent"
                        />
                      )}
                      <item.icon {...navIconProps} />
                      {item.label}
                    </>
                  )}
                </NavLink>
              ) : (
                <span
                  aria-disabled="true"
                  title="Not available yet"
                  className="flex h-10 cursor-default items-center gap-[10px] pl-[14px] pr-4 text-[15px] text-muted/50"
                >
                  <item.icon {...navIconProps} />
                  {item.label}
                </span>
              )}
            </li>
          ))}
        </ul>
      </nav>

      <div className="flex items-center justify-between border-t border-border px-4 py-3">
        <AuthControl />
        <ThemeToggle isDark={isDark} onToggle={onToggleTheme} />
      </div>
    </aside>
  );
}

function MobileHeader({
  isDark,
  onToggleTheme,
}: {
  isDark: boolean;
  onToggleTheme: () => void;
}) {
  return (
    <header className="sticky top-0 z-20 flex h-16 items-center border-b border-border bg-bg px-4 lg:hidden">
      <Brand />
      <nav aria-label="Main" className="ml-auto flex items-center gap-2">
        <NavLink
          to="schedule"
          className={({ isActive }) =>
            `flex items-center gap-2 rounded-[6px] px-2 py-2 text-[15px] transition-colors ${
              isActive ? "font-medium text-text" : "text-muted hover:text-text"
            }`
          }
        >
          <Clock {...navIconProps} />
          <span className="hidden sm:inline">Schedule</span>
        </NavLink>
        <AuthControl />
        <ThemeToggle isDark={isDark} onToggle={onToggleTheme} />
      </nav>
    </header>
  );
}

export function AppLayout() {
  const [isDark, setIsDark] = useState(() =>
    document.documentElement.classList.contains("dark"),
  );

  const toggleTheme = () => {
    const nextIsDark = !document.documentElement.classList.contains("dark");

    document.documentElement.classList.toggle("dark", nextIsDark);
    setIsDark(nextIsDark);

    try {
      localStorage.setItem(themeStorageKey, nextIsDark ? "dark" : "light");
    } catch {
      return;
    }
  };

  return (
    <div className="min-h-svh lg:flex">
      <Sidebar isDark={isDark} onToggleTheme={toggleTheme} />

      <div className="flex min-w-0 flex-1 flex-col">
        <MobileHeader isDark={isDark} onToggleTheme={toggleTheme} />

        <main className="flex-1 px-4 py-6 sm:px-6 lg:px-10 lg:py-10">
          <div className="mx-auto w-full max-w-[72rem]">
            <Outlet />
          </div>
        </main>

        <footer className="border-t border-border px-4 py-4 sm:px-6 lg:px-10">
          <div className="mx-auto flex w-full max-w-[72rem] flex-wrap items-center gap-x-4 gap-y-1">
            <p className="text-[13px] text-muted">
              StageFlow · Festival Operations Platform
            </p>
            <p className="text-[13px] text-muted sm:ml-auto">© 2026 faaazy</p>
          </div>
        </footer>
      </div>
    </div>
  );
}
