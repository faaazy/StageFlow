import { NavLink, Outlet } from "react-router-dom";
import { useState } from "react";

type NavIconName =
  | "cart"
  | "clock"
  | "flag"
  | "layers"
  | "layout"
  | "mic"
  | "moon"
  | "sun"
  | "ticket";

const iconPaths: Record<NavIconName, React.ReactNode> = {
  layout: (
    <>
      <rect x="3" y="3" width="7" height="7" rx="1.5" />
      <rect x="14" y="3" width="7" height="7" rx="1.5" />
      <rect x="14" y="14" width="7" height="7" rx="1.5" />
      <rect x="3" y="14" width="7" height="7" rx="1.5" />
    </>
  ),
  flag: (
    <>
      <path d="M4.5 21V3" />
      <path d="M4.5 4.5h12l-2.25 4.5L16.5 13.5h-12" />
    </>
  ),
  clock: (
    <>
      <circle cx="12" cy="12" r="8.5" />
      <path d="M12 7.25V12l3 1.75" />
    </>
  ),
  mic: (
    <>
      <rect x="9" y="2.5" width="6" height="11" rx="3" />
      <path d="M5.5 11a6.5 6.5 0 0 0 13 0" />
      <path d="M12 17.5v4" />
    </>
  ),
  layers: (
    <>
      <path d="M12 2.75 21 7.5l-9 4.75L3 7.5l9-4.75Z" />
      <path d="m3 12.5 9 4.75 9-4.75" />
    </>
  ),
  ticket: (
    <>
      <path d="M3 8.5V6.5a1 1 0 0 1 1-1h16a1 1 0 0 1 1 1v2a2.5 2.5 0 0 0 0 5v2a1 1 0 0 1-1 1H4a1 1 0 0 1-1-1v-2a2.5 2.5 0 0 0 0-5Z" />
      <path d="M12 6v2M12 11v2M12 16v2" />
    </>
  ),
  cart: (
    <>
      <circle cx="9.5" cy="20" r="1" />
      <circle cx="17.5" cy="20" r="1" />
      <path d="M2.5 3.5h2.75l2.35 11.6a1.5 1.5 0 0 0 1.48 1.15h8.4a1.5 1.5 0 0 0 1.47-1.18L21 7H6" />
    </>
  ),
  sun: (
    <>
      <circle cx="12" cy="12" r="4" />
      <path d="M12 2v2M12 20v2M4.9 4.9l1.4 1.4M17.7 17.7l1.4 1.4M2 12h2M20 12h2M4.9 19.1l1.4-1.4M17.7 6.3l1.4-1.4" />
    </>
  ),
  moon: <path d="M20.5 14.3A8.5 8.5 0 0 1 9.7 3.5a8.5 8.5 0 1 0 10.8 10.8Z" />,
};

function NavIcon({ name }: { name: NavIconName }) {
  return (
    <svg
      aria-hidden="true"
      viewBox="0 0 24 24"
      fill="none"
      stroke="currentColor"
      strokeWidth={1.5}
      strokeLinecap="round"
      strokeLinejoin="round"
      className="size-[18px] shrink-0"
    >
      {iconPaths[name]}
    </svg>
  );
}

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
      <NavIcon name={isDark ? "sun" : "moon"} />
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

const navItems: { label: string; to?: string; icon: NavIconName }[] = [
  { label: "Dashboard", icon: "layout" },
  { label: "Festivals", icon: "flag" },
  { label: "Schedule", to: "schedule", icon: "clock" },
  { label: "Artists", icon: "mic" },
  { label: "Stages", icon: "layers" },
  { label: "Tickets", icon: "ticket" },
  { label: "Orders", icon: "cart" },
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
          {navItems.map((item) =>
            item.to ? (
              <li key={item.label}>
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
                      <NavIcon name={item.icon} />
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
              </li>
            ) : (
              <li key={item.label}>
                <span
                  aria-disabled="true"
                  title="Not available yet"
                  className="flex cursor-default items-center gap-2.5 rounded px-2 py-2 text-[15px] text-neutral-400 dark:text-neutral-600"
                >
                  <NavIcon name={item.icon} />
                  {item.label}
                </span>
              </li>
            ),
          )}
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
              <NavIcon name="clock" />
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
