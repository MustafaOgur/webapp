import React from 'react';
import ReactMarkdown from 'react-markdown';
import remarkGfm from 'remark-gfm';
import { Prism as SyntaxHighlighter } from 'react-syntax-highlighter';
import { vscDarkPlus } from 'react-syntax-highlighter/dist/esm/styles/prism';

const MarkdownRenderer = ({ content }) => {
  return (
    <div className="markdown-body" style={{ fontSize: '0.95rem', lineHeight: '1.6' }}>
      <ReactMarkdown
        children={content}
        remarkPlugins={[remarkGfm]} // Tabloları desteklemek için
        components={{
          // Kod bloklarını özelleştiriyoruz
          code({ node, inline, className, children, ...props }) {
            const match = /language-(\w+)/.exec(className || '');
            return !inline && match ? (
              <div className="rounded-3 overflow-hidden my-3 shadow-sm">
                 {/* Kod Başlığı (Opsiyonel: Dil Adı) */}
                 <div className="bg-dark text-light px-3 py-1 small d-flex justify-content-between align-items-center" style={{opacity: 0.8}}>
                    <span>{match[1].toUpperCase()}</span>
                 </div>
                 {/* Kodun Kendisi */}
                 <SyntaxHighlighter
                    style={vscDarkPlus}
                    language={match[1]}
                    PreTag="div"
                    {...props}
                    customStyle={{ margin: 0, padding: '15px' }}
                  >
                    {String(children).replace(/\n$/, '')}
                  </SyntaxHighlighter>
              </div>
            ) : (
              // Satır içi kod (örn: `değişken`)
              <code className="bg-light text-danger px-1 rounded fw-bold" {...props}>
                {children}
              </code>
            );
          },
          // Tabloları Bootstrap stiliyle sarmalayalım
          table({ children }) {
            return (
                <div className="table-responsive my-3">
                    <table className="table table-bordered table-striped table-hover bg-white">
                        {children}
                    </table>
                </div>
            );
          },
          // Linkleri yeni sekmede aç
          a({ href, children }) {
             return <a href={href} target="_blank" rel="noopener noreferrer" className="text-primary text-decoration-underline">{children}</a>
          }
        }}
      >
        {content}
      </ReactMarkdown>
    </div>
  );
};

export default MarkdownRenderer;