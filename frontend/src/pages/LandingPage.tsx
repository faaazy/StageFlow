import { Clock, Mic, Ticket, type LucideIcon } from "lucide-react";
import { Link } from "react-router-dom";

const chipClassName =
  "rounded-[4px] border border-border px-2 py-[2px] text-[13px] font-medium text-text";

const primaryButtonClassName =
  "inline-flex h-11 w-auto items-center justify-center rounded-[6px] bg-accent px-6 text-[15px] font-semibold text-accent-fg transition-[filter] hover:brightness-95 focus-visible:ring-2 focus-visible:ring-accent focus-visible:outline-none dark:hover:brightness-110";

const secondaryButtonClassName =
  "inline-flex h-11 items-center justify-center rounded-[6px] border border-border px-6 text-[14px] text-text transition-colors hover:bg-surface focus-visible:ring-2 focus-visible:ring-accent focus-visible:outline-none";

const cardIconProps = {
  "aria-hidden": true,
  strokeWidth: 1.5,
  className: "size-5 shrink-0 text-accent",
};

const features: {
  icon: LucideIcon;
  title: string;
  description: string;
  comingSoon?: boolean;
}[] = [
  {
    icon: Clock,
    title: "Full running order",
    description:
      "Every set by stage and time, shown in the festival’s own time zone.",
  },
  {
    icon: Mic,
    title: "Stages and artists",
    description: "See who plays on which stage, with no overlaps and no guesswork.",
  },
  {
    icon: Ticket,
    title: "Tickets",
    description: "Buy tickets and keep them in one place, with a QR code for entry.",
    comingSoon: true,
  },
];

export function LandingPage() {
  return (
    <div className="mx-auto w-full max-w-[72rem] px-4 pt-14 pb-20 sm:px-6 lg:px-10 lg:pt-24">
      <section aria-labelledby="hero-title">
        <p className="mb-4 text-[12px] font-medium uppercase tracking-wider text-muted">
          Festival operations &amp; ticketing
        </p>
        <h1
          id="hero-title"
          className="max-w-[20ch] text-4xl leading-[1.1] font-semibold tracking-tight lg:text-[56px]"
        >
          Know who plays, where and when.
        </h1>
        <p className="mt-5 max-w-[38rem] text-[18px] leading-relaxed text-muted">
          StageFlow brings every festival’s running order together: stages,
          artists and set times in one place.
        </p>

        <div className="mt-8 flex max-w-[15rem] flex-wrap items-center gap-3 sm:max-w-none">
          <Link to="/register" className={primaryButtonClassName}>
            Create account
          </Link>
          <Link to="/login" className={secondaryButtonClassName}>
            Sign in
          </Link>
        </div>

        <div className="mt-4 flex flex-wrap items-center gap-2 text-[13px] text-muted">
          <span className={chipClassName}>Coming soon</span>
          <span>Public programs and ticket sales</span>
        </div>
      </section>

      <section aria-labelledby="features-title" className="mt-20 lg:mt-28">
        <h2 id="features-title" className="mb-6 text-[24px] font-semibold">
          What you get
        </h2>
        <div className="grid gap-4 md:grid-cols-3">
          {features.map(({ icon: Icon, title, description, comingSoon }) => (
            <article
              key={title}
              className="rounded-[6px] border border-border bg-surface p-5"
            >
              <Icon {...cardIconProps} />
              <div className="mt-4 flex items-center justify-between gap-2">
                <h3 className="text-[17px] font-semibold">{title}</h3>
                {comingSoon && (
                  <span className={chipClassName}>Coming soon</span>
                )}
              </div>
              <p className="mt-2 text-[15px] leading-relaxed text-muted">
                {description}
              </p>
            </article>
          ))}
        </div>
      </section>

      <section
        aria-labelledby="organizers-title"
        className="mt-20 rounded-[6px] border border-border bg-surface p-6 sm:p-8 lg:mt-28 md:flex md:items-center md:justify-between md:gap-8"
      >
        <div>
          <h2 id="organizers-title" className="text-[24px] font-semibold">
            Organizing a festival?
          </h2>
          <p className="mt-2 max-w-[34rem] text-[15px] leading-relaxed text-muted">
            Create festivals, add stages and artists, and build a schedule that
            never double-books a stage.
          </p>
        </div>
        <div className="mt-5 md:mt-0 md:shrink-0">
          <Link to="/organizer" className={secondaryButtonClassName}>
            For organizers
          </Link>
        </div>
      </section>
    </div>
  );
}
