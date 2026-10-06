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
      className="grid size-8 shrink-0 place-items-center rounded border border-neutral-200 text-neutral-500 transition-colors hover:bg-neutral-100 hover:text-neutral-900 focus-visible:ring-2 focus-visible:ring-accent-600 focus-visible:outline-none dark:border-neutral-700 dark:bg-neutral-900 dark:text-neutral-300 dark:hover:bg-neutral-800 dark:hover:text-neutral-50 dark:focus-visible:ring-accent-400"
    >
      {isDark ? <Sun {...navIconProps} /> : <Moon {...navIconProps} />}
    </button>
  );
}

function Brand() {
  return (
    <span className="inline-flex items-center rounded border border-neutral-300 px-3.5 py-1.5 dark:border-neutral-700">
      <span className="text-[15px] font-bold tracking-[0.06em]">
        <span className="text-accent-600 dark:text-accent-400">STAGE</span>
        <span className="text-neutral-900 dark:text-neutral-50">FLOW</span>
      </span>
    </span>
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
    <aside className="sticky top-0 hidden h-svh w-64 shrink-0 flex-col border-r border-neutral-200 bg-white lg:flex dark:border-neutral-800 dark:bg-neutral-950">
      <div className="flex h-16 items-center border-b border-neutral-200 px-5 dark:border-neutral-800">
        <Brand />
      </div>

      <nav aria-label="Main" className="flex-1 overflow-y-auto px-3 py-5">
        <p className="px-2 pb-2 text-xs font-medium tracking-wider text-neutral-400 uppercase dark:text-neutral-400">
          Operations
        </p>
        <ul className="space-y-1">
          {navItems.map((item) => (
            <li key={item.label}>
              {item.to ? (
                <NavLink
                  to={item.to}
                  className={({ isActive }) =>
                    `flex items-center gap-2.5 rounded px-2 py-2 text-[15px] transition-colors ${
                      isActive
                        ? "bg-accent-50 font-medium text-accent-800 dark:bg-accent-500/15 dark:font-medium dark:text-accent-100"
                        : "text-neutral-600 hover:bg-neutral-100 hover:text-neutral-900 dark:text-neutral-400 dark:hover:bg-neutral-800/70 dark:hover:text-neutral-100"
                    }`
                  }
                >
                  {({ isActive }) => (
                    <>
                      <item.icon {...navIconProps} />
                      {item.label}
                      {isActive && (
                        <span
                          aria-hidden="true"
                          className="ml-auto size-1.5 rounded-full bg-accent-600 dark:bg-accent-400"
                        />
                      )}
                    </>
                  )}
                </NavLink>
              ) : (
                <span
                  aria-disabled="true"
                  title="Not available yet"
                  className="flex cursor-default items-center gap-2.5 rounded px-2 py-2 text-[15px] text-neutral-400 dark:text-neutral-600"
                >
                  <item.icon {...navIconProps} />
                  {item.label}
                </span>
              )}
            </li>
          ))}
        </ul>
      </nav>

      <div className="flex items-center justify-end border-t border-neutral-200 px-5 py-3 dark:border-neutral-800">
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
    <header className="sticky top-0 z-20 flex h-16 items-center border-b border-neutral-200 bg-white px-4 lg:hidden dark:border-neutral-800 dark:bg-neutral-950">
      <Brand />
      <nav aria-label="Main" className="ml-auto flex items-center gap-2">
        <ul>
          <li>
            <NavLink
              to="schedule"
              className={({ isActive }) =>
                `flex items-center gap-2 rounded px-2 py-2 text-[15px] ${
                  isActive
                    ? "bg-accent-50 font-medium text-accent-800 dark:bg-accent-500/15 dark:text-accent-100"
                    : "text-neutral-600 dark:text-neutral-400"
                }`
              }
            >
              <Clock {...navIconProps} />
              <span className="hidden sm:inline">Schedule</span>
            </NavLink>
          </li>
        </ul>
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

        <main className="flex-1 px-4 py-6 sm:px-6 lg:px-8 lg:py-8">
          <div className="mx-auto w-full max-w-6xl">
            <Outlet />
          </div>
        </main>

        <footer className="border-t border-neutral-200 px-4 py-4 sm:px-6 lg:px-8 dark:border-neutral-800">
          <div className="mx-auto flex w-full max-w-6xl flex-wrap items-center gap-x-4 gap-y-1">
            <p className="text-[13px] text-neutral-500 dark:text-neutral-400">
              StageFlow · Festival Operations Platform
            </p>
            <p className="text-[13px] text-neutral-400 sm:ml-auto dark:text-neutral-500">
              © 2026 faaazy
            </p>
          </div>
        </footer>
      </div>
    </div>
  );
}
