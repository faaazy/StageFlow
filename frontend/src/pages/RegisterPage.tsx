import { useMutation } from "@tanstack/react-query";
import { useState, type SubmitEvent } from "react";
import { Link, useNavigate } from "react-router-dom";
import { register } from "../api/authApi";
import { AuthCard } from "../components/AuthCard";
import {
  errorBlockClassName,
  inputClassName,
  labelClassName,
  primaryButtonClassName,
} from "../components/formStyles";

export function RegisterPage() {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");

  const navigate = useNavigate();

  const mutation = useMutation({
    mutationFn: ({ email, password }: { email: string; password: string }) =>
      register(email, password),
    onSuccess: () => {
      navigate("/login");
    },
  });

  const registerFormHandler = (e: SubmitEvent<HTMLFormElement>) => {
    e.preventDefault();

    mutation.mutate({ email, password });
  };

  return (
    <AuthCard eyebrow="New account" title="Sign up">
      <form onSubmit={(e) => registerFormHandler(e)}>
        <div className="space-y-4">
          <div>
            <label htmlFor="email" className={labelClassName}>
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
            <label htmlFor="password" className={labelClassName}>
              Password
            </label>
            <input
              id="password"
              type="password"
              autoComplete="new-password"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              className={inputClassName}
            />
          </div>
        </div>

        <button
          type="submit"
          disabled={mutation.isPending}
          className={primaryButtonClassName}
        >
          {mutation.isPending ? "Signing up…" : "Sign up"}
        </button>

        {mutation.isError && (
          <div role="status" aria-live="polite" className={errorBlockClassName}>
            <p className="font-semibold text-conflict">Error</p>
            <p className="mt-0.5 break-words text-[15px] text-text">
              {mutation.error?.message}
            </p>
          </div>
        )}
      </form>

      <p className="mt-5 text-[14px] text-muted">
        Already have an account?{" "}
        <Link
          to="/login"
          className="rounded-[2px] text-text underline underline-offset-2 hover:text-accent focus-visible:ring-2 focus-visible:ring-accent focus-visible:outline-none"
        >
          Sign in
        </Link>
      </p>
    </AuthCard>
  );
}
