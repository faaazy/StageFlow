import { Moon, Sun } from "lucide-react";
import { useTheme } from "../theme/useTheme";

const themeIconProps = {
  "aria-hidden": true,
  strokeWidth: 1.5,
  className: "size-[18px] shrink-0",
};

export function ThemeToggle() {
  const { isDark, toggleTheme } = useTheme();

  const label = isDark ? "Switch to light theme" : "Switch to dark theme";

  return (
    <button
      type="button"
      onClick={toggleTheme}
      aria-label={label}
      title={label}
      aria-pressed={isDark}
      className="grid size-8 shrink-0 place-items-center rounded-[6px] border border-border text-muted transition-colors hover:text-text focus-visible:ring-2 focus-visible:ring-accent focus-visible:outline-none"
    >
      {isDark ? <Sun {...themeIconProps} /> : <Moon {...themeIconProps} />}
    </button>
  );
}
