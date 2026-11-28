import type { ReactNode } from "react";

function Spinner() {
  return (
    <span
      aria-hidden="true"
      className="size-4 animate-spin rounded-full border-2 border-current border-t-transparent"
    />
  );
}

export function StatusAlert({
  tone,
  title,
  children,
}: {
  tone: "error" | "conflict" | "pending" | "success";
  title?: string;
  children: ReactNode;
}) {
  const toneClassName = {
    error: "border-l-conflict",
    conflict: "border-l-conflict",
    pending: "border-l-border",
    success: "border-l-accent",
  }[tone];
  const titleClassName = {
    error: "text-conflict",
    conflict: "text-conflict",
    pending: "text-muted",
    success: "text-accent",
  }[tone];

  return (
    <div
      role="status"
      aria-live="polite"
      className={`flex items-start gap-2.5 rounded-[4px] border-l-2 bg-surface px-[14px] py-3 text-[15px] leading-relaxed ${toneClassName}`}
    >
      {tone === "pending" && (
        <span className={`mt-0.5 ${titleClassName}`}>
          <Spinner />
        </span>
      )}
      <div className="min-w-0">
        {title && <p className={`font-semibold ${titleClassName}`}>{title}</p>}
        <p
          className={
            title ? "mt-0.5 break-words text-text" : "break-words text-text"
          }
        >
          {children}
        </p>
      </div>
    </div>
  );
}
