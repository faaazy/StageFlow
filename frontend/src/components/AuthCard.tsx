import type { ReactNode } from "react";

type AuthCardProps = {
  eyebrow: string;
  title: string;
  children: ReactNode;
};

export function AuthCard({ eyebrow, title, children }: AuthCardProps) {
  return (
    <div className="mx-auto flex w-full max-w-[72rem] justify-center px-4 py-12 sm:px-6 sm:py-20 lg:px-10">
      <div className="w-full max-w-[420px] rounded-[2px] border border-border bg-surface">
        <div className="border-b border-border bg-bg px-6 py-3 font-mono text-[12px] uppercase tracking-wider text-muted">
          {eyebrow}
        </div>
        <div className="p-6 sm:p-8">
          <h1 className="font-display text-[clamp(2.25rem,9vw,3.25rem)] font-black uppercase leading-[0.95] tracking-tight">
            {title}
          </h1>
          <div className="mt-6">{children}</div>
        </div>
      </div>
    </div>
  );
}
