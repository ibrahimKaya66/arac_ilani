import { MetadataRoute } from "next";

const siteUrl = process.env.NEXT_PUBLIC_SITE_URL ?? "https://aracilan.com";

export default function robots(): MetadataRoute.Robots {
  return {
    rules: { userAgent: "*", allow: "/", disallow: ["/benim-ilanlarim", "/ilan-ver"] },
    sitemap: `${siteUrl}/sitemap.xml`,
  };
}
