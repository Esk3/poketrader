import { ApiUrl } from '$lib';
import type { HandleFetch } from '@sveltejs/kit';

export const handleFetch: HandleFetch = async ({ event, request, fetch }) => {
  const url = new URL(request.url);

  console.log(url, ApiUrl);
  if (url.pathname.startsWith("/API") || url.pathname.startsWith(ApiUrl.pathname)) {
    url.pathname = url.pathname.replace("/API", "");
    url.host = ApiUrl.host;
    request = new Request(url, request);
    request.headers.set("cookie", event.request.headers.get("cookie"));
  }

  return await fetch(request);
};

