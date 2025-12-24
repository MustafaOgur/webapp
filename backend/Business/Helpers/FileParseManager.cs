using Markdig;
using Markdig.Syntax;

namespace Business.Helpers
{
    public static class FileParseManager
    {
        public static string? GetFileExtension(string content)
        {
            if (string.IsNullOrEmpty(content)) return null;

            // Markdig pipeline oluştur
            var pipeline = new MarkdownPipelineBuilder().Build();
            // İçeriği analiz et (Parse)
            var document = Markdown.Parse(content, pipeline);

            // Dökümandaki her bloğu gez
            foreach (var node in document)
            {
                // Eğer bu blok bir "FencedCodeBlock" ise (yani ``` ile başlıyorsa)
                if (node is FencedCodeBlock codeBlock && !string.IsNullOrEmpty(codeBlock.Info))
                {
                    // codeBlock.Info bize dili verir (örn: yaml, python, json)
                    var language = codeBlock.Info.ToLower().Trim();

                    // Dile göre uzantı döndür
                    return language switch
                    {
                        // --- Scripting & Programlama ---
                        "python" or "py" => ".py",
                        "bash" or "sh" or "shell" or "zsh" => ".sh",
                        "go" or "golang" => ".go",       // DevOps'un göz bebeği Go eklendi
                        "csharp" or "cs" or "dotnet" => ".cs",
                        "javascript" or "js" or "node" => ".js",
                        "typescript" or "ts" => ".ts",   // Pulumi veya NodeJS scriptleri için
                        "java" => ".java",
                        "groovy" or "jenkins" or "jenkinsfile" => ".groovy", // Jenkins Pipeline'ları için şart!
                        "powershell" or "ps1" or "pwsh" => ".ps1", // Azure ve Windows otomasyonu için

                        // --- IaC & Konfigürasyon (DevOps Özel) ---
                        "terraform" or "tf" or "hcl" => ".tf", // Terraform HCL formatında gelir, .tf olmalı
                        "yaml" or "yml" => ".yaml",      // Ansible, K8s, Docker Compose, Helm hepsi buraya düşer
                        "json" => ".json",               // CloudFormation veya genel config
                        "xml" or "maven" => ".xml",      // Maven veya eski configler
                        "ini" or "conf" or "config" or "toml" => ".conf", // Systemd servisleri veya ayar dosyaları
                        "properties" or "env" => ".env", // Ortam değişkenleri dosyaları

                        // --- Web & Veritabanı ---
                        "html" => ".html",
                        "css" => ".css",
                        "sql" or "mysql" or "pgsql" or "postgres" => ".sql",

                        // --- Özel Dosyalar ---
                        "dockerfile" or "docker" => "",  // Dockerfile'ın uzantısı olmaz (Dosya adı Dockerfile olur)
                        "makefile" or "make" => "",      // Makefile uzantısızdır
                        "markdown" or "md" => ".md",     // Dokümantasyon istenirse

                        // Bilinmeyen durumlar
                        _ => ".txt"
                    };
                }
            }

            // Hiç kod bloğu yoksa null dön (Normal sohbet)
            return null;
        }
    }
}