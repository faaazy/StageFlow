---
name: stageflow-ui
description: Visual style guide for the StageFlow frontend (React + Tailwind v4) - color tokens, fonts, brand logo, layout, buttons, inputs, alerts, list rows, auth card. Use whenever you create or restyle any page or component in frontend/, or when asked about StageFlow design, theme or styling. Do not use for backend or business logic work.
---

# StageFlow UI

Это описание ТЕКУЩЕГО оформления фронтенда StageFlow (`frontend/`). Источник истины это код: токены в `frontend/src/index.css`, образцы в `src/layouts/AppLayout.tsx`, `src/pages/LoginPage.tsx`, `src/pages/SchedulePage.tsx`. Если код и этот файл расходятся, прав код; сообщи об этом и предложи обновить навык.

Работа с оформлением никогда не должна менять бизнес-логику: запросы к API (`src/api/*`), хранение токена (`src/auth/*`), хуки `useQuery`/`useMutation`, обработчики, состояние, типы и тексты, на которые опираются тесты.

## Цвета

Только семантические токены, объявленные в `index.css` (`:root` для светлой темы, `.dark` для тёмной, подключены через `@theme inline`). Пользуйся классами `bg-bg`, `bg-surface`, `border-border`, `text-text`, `text-muted`, `bg-accent`, `text-accent`, `text-accent-fg`, `text-conflict`, `border-l-conflict`, `text-cool`.

| токен     | светлая | тёмная  | назначение                             |
| --------- | ------- | ------- | -------------------------------------- |
| bg        | #F4F6F7 | #0B0F14 | фон страницы, сайдбар, шапки таблиц    |
| surface   | #FFFFFF | #141B23 | карточки, панели, строки списков       |
| border    | #D5DDE3 | #26313C | все границы и разделители              |
| text      | #0E141A | #E6EEF2 | основной текст                         |
| muted     | #5B6772 | #8A99A6 | подписи, второстепенный текст          |
| accent    | #3F6B0A | #C8F169 | кнопки, активный пункт, часть логотипа |
| accent-fg | #FFFFFF | #0B0F14 | текст НА акцентной кнопке              |
| conflict  | #C4262E | #FF5A5F | ошибки и конфликты                     |
| cool      | #2F7A8F | #5FB4C9 | информационный цвет                    |

Правила:

- Не использовать `neutral-*`, `gray-*`, `white`, `black` и произвольные hex в разметке. Если нужного цвета нет, добавь токен в `index.css`, а не пиши значение в компоненте.
- Не дублировать пары `X dark:Y`: токен сам меняется по теме. Единственное принятое исключение это фон полей ввода: `bg-surface dark:bg-bg`.
- Светлый акцент намеренно темнее тёмного (лайм не читается на светлом фоне). Не заменять один другим.
- Оттенок при hover строк: `hover:bg-[color-mix(in_srgb,var(--text)_6%,var(--surface))]`.

## Тема

Тёмная тема это класс `dark` на `<html>`. Предустановка и переключатель (`ThemeToggle`, ключ `stageflow-theme` в localStorage, скрипт в `index.html`) уже работают. Не менять механизм: только оформление кнопки.

## Шрифты

Подключены через `@fontsource` в `index.css`.

- Заголовки (`h1`-`h3`, логотип): Commissioner 600, класс `font-heading` (для `h1`-`h3` задан в базовом слое автоматически).
- Основной текст: Golos Text 400/500/600 (`font-sans` по умолчанию), базовый размер 15px, мелкий текст 13px.
- Время, даты, длительность, счётчики, UUID: JetBrains Mono (`font-mono`) вместе с `tabular-nums`.

## Логотип (намеренное решение)

`STAGEFLOW`, шрифт heading, 15px, semibold, `tracking-[0.06em]`, БЕЗ рамки. Слово **STAGE** цветом `text-text`, слово **FLOW** цветом `text-accent`. Так задумано автором проекта: не менять местами и не «исправлять» под макет «Пульт». Эталон: компонент `Brand` в `AppLayout.tsx`, тот же вид на странице входа.

## Размеры и форма

- Скругления: 6px для карточек, полей и кнопок, 4px для чипов и блоков сообщений, 8px для карточки входа.
- Без теней и градиентов. Глубину задают границы `border-border` и фон `surface` против `bg`.
- Контейнер контента: `max-w-[72rem]`, отступы `px-4 sm:px-6 lg:px-10`, `py-6 lg:py-10`.

## Компоненты

Копируй классы из существующих файлов, а не придумывай заново.

**Карточка / панель:** `rounded-[6px] border border-border bg-surface`, внутренний отступ `p-5`.

**Подпись поля:** `mb-[6px] block text-[13px] text-muted`, всегда связана с полем через `htmlFor`/`id`.

**Поле ввода:** `block h-10 w-full rounded-[6px] border border-border bg-surface px-3 text-[15px] text-text transition-colors placeholder:text-muted focus-visible:ring-2 focus-visible:ring-accent focus-visible:outline-none dark:bg-bg`. Для UUID добавлять `font-mono text-[13px]`.

**Основная кнопка:** `inline-flex h-11 w-full items-center justify-center rounded-[6px] bg-accent px-4 text-[15px] font-semibold text-accent-fg transition-[filter] hover:brightness-95 dark:hover:brightness-110 focus-visible:ring-2 focus-visible:ring-accent focus-visible:outline-none disabled:cursor-not-allowed disabled:opacity-50`. Одна основная кнопка на форму.

**Вторичная кнопка / ссылка-кнопка** (пример `SignInLink`): `inline-flex h-9 items-center justify-center rounded-[6px] border border-border px-4 text-[14px] text-text transition-colors hover:bg-surface focus-visible:ring-2 focus-visible:ring-accent focus-visible:outline-none`.

**Блок сообщения** (`StatusAlert` в `SchedulePage.tsx`): `rounded-[4px] border-l-2 bg-surface px-[14px] py-3 text-[15px]`, `role="status"`, `aria-live="polite"`. Цвет левой полосы и заголовка (semibold): ошибка и конфликт это `conflict`, успех это `accent`, ожидание это `border`/`muted` со спиннером. Текст сообщения всегда `text-text`.

**Чип** (название сцены): `rounded-[4px] border border-border px-2 py-[2px] text-[13px] font-medium text-text`.

**Спиннер:** `size-4 animate-spin rounded-full border-2 border-current border-t-transparent`. **Skeleton:** блоки `bg-border` с `animate-pulse`.

**Кнопка темы:** `size-8`, `rounded-[6px] border border-border text-muted hover:text-text`, иконки lucide 18px, `strokeWidth` 1.5.

## Шаблон страницы с данными (образец SchedulePage)

- Шапка: `h1` `text-[32px] font-semibold tracking-tight`, под ним описание `text-[15px] text-muted`; справа моноширинный счётчик `font-mono text-[13px] text-muted`; ниже линия `border-b border-border pb-4`.
- Контент в две колонки на `xl`: список слева, панель формы справа шириной `21rem` (`xl:sticky xl:top-8`); на узких экранах друг под другом.
- Список: карточка `overflow-hidden`, заголовок группы на `bg-bg` (дата `font-mono text-[14px] font-bold`, справа счётчик `text-[12px] uppercase tracking-wider text-muted`), строки разделены `divide-y divide-border`, `px-5 py-[14px]`.
- Строка выступления: колонки `sm:grid-cols-[170px_minmax(0,1fr)_auto]`: время `font-mono text-[18px] tabular-nums` и длительность под ним `font-mono text-[13px] text-muted`; название `text-[16px] font-semibold`; справа чип сцены.
- Состояния всегда все четыре: загрузка (skeleton), ошибка (`StatusAlert`), пусто (карточка с пунктирной границей `border-dashed`, `py-16`, по центру), данные.

## Шаблон страницы входа / регистрации (образец LoginPage)

Страница БЕЗ сайдбара, маршрут вне `AppLayout`. Фон `bg-bg`, карточка по центру (`grid min-h-svh place-items-center px-4`), `w-full max-w-[390px] rounded-[8px] border border-border bg-surface p-7`. Сверху логотип, затем `h1` `mt-5 text-[28px] font-semibold`, форма `mt-6` с полями `space-y-4`, основная кнопка `mt-5`, сообщение об ошибке `mt-4` под кнопкой.

## Боковое меню и шапка (образец AppLayout)

- Сайдбар: `sticky top-0 hidden h-svh w-56 shrink-0 flex-col border-r border-border bg-bg lg:flex`; мобильная шапка высотой `h-16` с нижней границей.
- Пункт меню: высота `h-10`, 15px, иконка lucide 18px, `gap-[10px]`; обычный `text-muted hover:text-text`; активный на `bg-surface` с полосой слева `absolute inset-y-0 left-0 w-[2px] bg-accent`; недоступный `text-muted/50`, `cursor-default`, не кликабельный.
- Низ сайдбара: граница сверху, ссылка «Sign in» слева, `ThemeToggle` справа.

## Доступность и анимации

- Видимый фокус у всех интерактивных элементов (`focus-visible:ring-2 focus-visible:ring-accent`).
- Каждое поле имеет `label`; иконки без текста имеют `aria-label`, декоративные иконки `aria-hidden`.
- `prefers-reduced-motion` уже учтён в базовом слое `index.css`: не отключать его.
- Контраст текста не ниже WCAG AA в обеих темах.

## Перед завершением работы

1. Проверь обе темы и узкий экран (~375px).
2. `npm run build`, `npm run lint` (если скрипт есть), `npm test`: существующие тесты должны проходить без правок.
3. Перечисли изменённые файлы и всё, что пришлось изменить вне чистого оформления.

## Публичный layout и лендинг

- Публичные страницы работают через `PublicLayout` + `Outlet`, без сайдбара и без переключателя темы: тема берётся из сохранённой на `<html>`.
- Обёртка: `flex min-h-svh flex-col bg-bg text-text`, `main` имеет `flex-1`; контейнер и отступы задаёт сама страница.
- Шапка: `sticky top-0 z-20 h-16 border-b border-border bg-bg`, внутри `mx-auto flex h-full w-full max-w-[72rem] items-center px-4 sm:px-6 lg:px-10`.
- Слева `Brand` в ссылке на `/`; справа `ml-auto flex items-center gap-2 sm:gap-4`: текстовая ссылка «For organizers» (15px, `text-muted hover:text-text`) и `AuthControl`.
- Подвал: `border-t border-border`, тот же контейнер с `py-6`, текст «StageFlow · Festival Operations Platform» 13px `text-muted`.
- Лендинг: контейнер `mx-auto w-full max-w-[72rem] px-4 pt-14 pb-20 sm:px-6 lg:px-10 lg:pt-24`, секции разделены `mt-20 lg:mt-28`.
- Hero: eyebrow 12px uppercase `tracking-wider text-muted`, `h1` 36/56px с `max-w-[20ch] leading-[1.1] tracking-tight`, абзац 18px `text-muted max-w-[38rem]`.
- Кнопки hero: основная `w-auto h-11 px-6`, вторичная `h-11 px-6`, ряд `mt-8 flex flex-wrap items-center gap-3`; ниже 13px `text-muted` с чипом «Coming soon».
- «What you get»: `h2` 24px и `grid gap-4 md:grid-cols-3`; карточки `rounded-[6px] border border-border bg-surface p-5`, иконка lucide 20px `text-accent` (`strokeWidth={1.5}`), `h3` 17px, абзац 15px `text-muted`.
- Чип «Coming soon» у карточки Tickets — тот же, что в hero: `rounded-[4px] border border-border px-2 py-[2px] text-[13px] font-medium text-text`.
- Панель «For organizers»: карточка `p-6 sm:p-8`, на `md` `flex items-center justify-between gap-8`; слева `h2` 24px и абзац 15px `max-w-[34rem]`, справа вторичная кнопка-ссылка `h-11 px-6 shrink-0`.
- У всех ссылок и кнопок `focus-visible:ring-2 focus-visible:ring-accent focus-visible:outline-none`; декоративные иконки скрыты через `aria-hidden`.
