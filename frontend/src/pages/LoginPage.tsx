import { useMutation } from "@tanstack/react-query";
import { ArrowLeft } from "lucide-react";
import { useState, type SubmitEvent } from "react";
import { login } from "../api/authApi";
import { setToken } from "../auth/tokenStorage";
import { Link, useNavigate } from "react-router-dom";

const inputClassName =
  "block h-10 w-full rounded-[6px] border border-border bg-surface px-3 text-[15px] text-text transition-colors placeholder:text-muted focus-visible:ring-2 focus-visible:ring-accent focus-visible:outline-none dark:bg-bg";

const secondaryLinkClassName =
  "inline-flex h-9 w-full items-center justify-center rounded-[6px] border border-border px-4 text-[14px] text-text transition-colors hover:bg-surface focus-visible:ring-2 focus-visible:ring-accent focus-visible:outline-none";

const backLinkClassName =
  "grid size-8 shrink-0 place-items-center rounded-[6px] border border-border text-muted transition-colors hover:text-text focus-visible:ring-2 focus-visible:ring-accent focus-visible:outline-none";

export function LoginPage() {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");

  const navigate = useNavigate();

  const mutation = useMutation({
    mutationFn: ({ email, password }: { email: string; password: string }) =>
      login(email, password),
    onSuccess: (token) => {
      setToken(token);
      navigate("/schedule");
    },
  });

  const loginFormHandler = (e: SubmitEvent<HTMLFormElement>) => {
    e.preventDefault();

    mutation.mutate({ email, password });
  };

  return (
    <div className="grid min-h-svh place-items-center bg-bg px-4">
      <div className="w-full max-w-[390px] rounded-[8px] border border-border bg-surface p-7">
        <div className="flex items-center gap-3">
          <Link
            to="/"
            aria-label="Back to home"
            title="Back to home"
            className={backLinkClassName}
          >
            <ArrowLeft
              aria-hidden="true"
              strokeWidth={1.5}
              className="size-[18px]"
            />
          </Link>
          <p className="font-heading text-[15px] font-semibold tracking-[0.06em]">
            <span className="text-text">STAGE</span>
            <span className="text-accent">FLOW</span>
          </p>
        </div>
        <h1 className="mt-5 text-[28px] font-semibold">Sign in</h1>

        <form onSubmit={(e) => loginFormHandler(e)} className="mt-6">
          <div className="space-y-4">
            <div>
              <label
                htmlFor="email"
                className="mb-[6px] block text-[13px] text-muted"
              >
                Email
              </label>
              <input
                id="email"
                type="email"
                autoComplete="email"
                value={email}
                onChange={(e) => setEmail(e.target.value)}
                className={inputClassName}
              />
            </div>

            <div>
              <label
                htmlFor="password"
                className="mb-[6px] block text-[13px] text-muted"
              >
                Password
              </label>
              <input
                id="password"
                type="password"
                autoComplete="current-password"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
                className={inputClassName}
              />
            </div>
          </div>

          <button
            type="submit"
            disabled={mutation.isPending}
            className="mt-5 inline-flex h-11 w-full items-center justify-center rounded-[6px] bg-accent px-4 text-[15px] font-semibold text-accent-fg transition-[filter] hover:brightness-95 focus-visible:ring-2 focus-visible:ring-accent focus-visible:outline-none disabled:cursor-not-allowed disabled:opacity-50 dark:hover:brightness-110"
          >
            {mutation.isPending ? "Signing in…" : "Sign in"}
          </button>

          {mutation.isError && (
            <div
              role="status"
              aria-live="polite"
              className="mt-4 rounded-[4px] border-l-2 border-l-conflict bg-surface px-[14px] py-3"
            >
              <p className="font-semibold text-conflict">Error</p>
              <p className="mt-0.5 break-words text-[15px] text-text">
                {mutation.error?.message}
              </p>
            </div>
          )}
        </form>

        <nav aria-label="Account navigation" className="mt-6 border-t border-border pt-5">
          <p className="mb-3 text-[13px] text-muted">Don&apos;t have an account?</p>
          <Link to="/register" className={secondaryLinkClassName}>
            Sign up
          </Link>
        </nav>
      </div>
    </div>
  );
}
