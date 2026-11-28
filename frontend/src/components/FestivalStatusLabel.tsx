import type { FestivalStatus } from "../api/festivalsApi";

const statusStyles: Record<
  FestivalStatus,
  { label: string; dotClassName: string; textClassName: string }
> = {
  Draft: {
    label: "Draft",
    dotClassName: "bg-muted",
    textClassName: "text-text",
  },
  Published: {
    label: "Published",
    dotClassName: "bg-cool",
    textClassName: "text-text",
  },
  OnSale: {
    label: "On sale",
    dotClassName: "bg-text",
    textClassName: "text-text",
  },
  Live: {
    label: "Live",
    dotClassName: "bg-accent motion-safe:animate-pulse",
    textClassName: "text-text",
  },
  Finished: {
    label: "Finished",
    dotClassName: "bg-muted/60",
    textClassName: "text-muted/70",
  },
  Cancelled: {
    label: "Cancelled",
    dotClassName: "bg-muted",
    textClassName: "text-muted line-through",
  },
};

export function FestivalStatusLabel({ status }: { status: FestivalStatus }) {
  const { label, dotClassName, textClassName } = statusStyles[status];

  return (
    <span className={`inline-flex items-center gap-2 text-[14px] ${textClassName}`}>
      <span
        aria-hidden="true"
        className={`size-2 shrink-0 rounded-full ${dotClassName}`}
      />
      {label}
    </span>
  );
}
