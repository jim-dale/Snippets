namespace BulkWriterSourceGenerator;

using System;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// A thin wrapper over <see cref="StringBuilder" /> that adds indentation to each line built.
/// </summary>
internal class IndentedStringBuilder
{
    private const byte IndentSize = 4;
    private readonly StringBuilder stringBuilder = new();
    private int indent;
    private bool indentPending = true;

    /// <summary>
    /// Gets the current indent level.
    /// </summary>
    /// <value>The current indent level.</value>
    public virtual int IndentCount => this.indent;

    /// <summary>
    /// Gets the current length of the built string.
    /// </summary>
    public virtual int Length => this.stringBuilder.Length;

    /// <summary>
    /// Appends the current indent and then the given string to the string being built.
    /// </summary>
    /// <param name="value">The string to append.</param>
    /// <returns>This builder so that additional calls can be chained.</returns>
    public virtual IndentedStringBuilder Append(string value)
    {
        this.DoIndent();

        this.stringBuilder.Append(value);

        return this;
    }

    /// <summary>
    /// Appends the current indent and then the given string to the string being built.
    /// </summary>
    /// <param name="value">The string to append.</param>
    /// <returns>This builder so that additional calls can be chained.</returns>
    public virtual IndentedStringBuilder Append(FormattableString value)
    {
        this.DoIndent();

        this.stringBuilder.Append(value);

        return this;
    }

    /// <summary>
    /// Appends the current indent and then the given char to the string being built.
    /// </summary>
    /// <param name="value">The char to append.</param>
    /// <returns>This builder so that additional calls can be chained.</returns>
    public virtual IndentedStringBuilder Append(char value)
    {
        this.DoIndent();

        this.stringBuilder.Append(value);

        return this;
    }

    /// <summary>
    /// Appends the current indent and then the given strings to the string being built.
    /// </summary>
    /// <param name="value">The strings to append.</param>
    /// <returns>This builder so that additional calls can be chained.</returns>
    public virtual IndentedStringBuilder Append(IEnumerable<string> value)
    {
        this.DoIndent();

        foreach (var str in value)
        {
            this.stringBuilder.Append(str);
        }

        return this;
    }

    /// <summary>
    /// Appends the current indent and then the given chars to the string being built.
    /// </summary>
    /// <param name="value">The chars to append.</param>
    /// <returns>This builder so that additional calls can be chained.</returns>
    public virtual IndentedStringBuilder Append(IEnumerable<char> value)
    {
        this.DoIndent();

        foreach (var chr in value)
        {
            this.stringBuilder.Append(chr);
        }

        return this;
    }

    /// <summary>
    /// Appends a new line to the string being built.
    /// </summary>
    /// <returns>This builder so that additional calls can be chained.</returns>
    public virtual IndentedStringBuilder AppendLine()
    {
        this.stringBuilder.AppendLine();

        return this;
    }

    /// <summary>
    /// Appends the current indent, the given string, and a new line to the string being built.
    /// </summary>
    /// <remarks>
    /// If the given string itself contains a new line, the part of the string after that new line will not be indented.
    /// </remarks>
    /// <param name="value">The string to append.</param>
    /// <returns>This builder so that additional calls can be chained.</returns>
    public virtual IndentedStringBuilder AppendLine(string value)
    {
        if (value.Length != 0)
        {
            this.DoIndent();
        }

        this.stringBuilder.AppendLine(value);

        this.indentPending = true;

        return this;
    }

    /// <summary>
    /// Appends the current indent, the given string, and a new line to the string being built.
    /// </summary>
    /// <param name="value">The string to append.</param>
    /// <returns>This builder so that additional calls can be chained.</returns>
    public virtual IndentedStringBuilder AppendLine(FormattableString value)
    {
        this.DoIndent();

        this.stringBuilder.Append(value);

        this.indentPending = true;

        return this;
    }

    /// <summary>
    /// Separates the given string into lines, and then appends each line, prefixed
    /// by the current indent and followed by a new line, to the string being built.
    /// </summary>
    /// <param name="value">The string to append.</param>
    /// <param name="skipFinalNewline">If <see langword="true" />, then the terminating new line is not added after the last line.</param>
    /// <returns>This builder so that additional calls can be chained.</returns>
    public virtual IndentedStringBuilder AppendLines(string value, bool skipFinalNewline = false)
    {
        using (var reader = new StringReader(value))
        {
            var first = true;
            string? line;
            while ((line = reader.ReadLine()) != null)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    this.AppendLine();
                }

                if (line.Length != 0)
                {
                    this.Append(line);
                }
            }
        }

        if (!skipFinalNewline)
        {
            this.AppendLine();
        }

        return this;
    }

    /// <summary>
    /// Resets this builder ready to build a new string.
    /// </summary>
    /// <returns>This builder so that additional calls can be chained.</returns>
    public virtual IndentedStringBuilder Clear()
    {
        this.stringBuilder.Clear();
        this.indent = 0;

        return this;
    }

    /// <summary>
    /// Increments the indent.
    /// </summary>
    /// <returns>This builder so that additional calls can be chained.</returns>
    public virtual IndentedStringBuilder IncrementIndent()
    {
        this.indent++;

        return this;
    }

    /// <summary>
    /// Decrements the indent.
    /// </summary>
    /// <returns>This builder so that additional calls can be chained.</returns>
    public virtual IndentedStringBuilder DecrementIndent()
    {
        if (this.indent > 0)
        {
            this.indent--;
        }

        return this;
    }

    /// <summary>
    /// Creates a scoped indenter that will increment the indent, then decrement it when disposed.
    /// </summary>
    /// <returns>An indenter.</returns>
    public virtual IDisposable Indent() => new Indenter(this);

    /// <summary>
    /// Creates a scoped indenter that will increment the indent, then decrement it when disposed.
    /// </summary>
    /// <returns>An indenter.</returns>
    public virtual IDisposable CodeBlockIndent() => new CodeBlockIndenter(this);

    /// <summary>
    /// Temporarily disables all indentation. Restores the original indentation when the returned object is disposed.
    /// </summary>
    /// <returns>An object that restores the original indentation when disposed.</returns>
    public virtual IDisposable SuspendIndent() => new IndentSuspender(this);

    /// <summary>
    /// Clones this <see cref="IndentedStringBuilder" />, copying the built string and current indent level.
    /// </summary>
    /// <returns>New instance of <see cref="IndentedStringBuilder" />.</returns>
    public virtual IndentedStringBuilder Clone()
    {
        IndentedStringBuilder result = new();

        result.stringBuilder.Append(this.stringBuilder);
        result.indent = this.indent;
        result.indentPending = this.indentPending;

        return result;
    }

    /// <summary>
    /// Returns the built string.
    /// </summary>
    /// <returns>The built string.</returns>
    public override string ToString() => this.stringBuilder.ToString();

    private void DoIndent()
    {
        if (this.indentPending && this.indent > 0)
        {
            this.stringBuilder.Append(' ', this.indent * IndentSize);
        }

        this.indentPending = false;
    }

    private sealed class Indenter : IDisposable
    {
        private readonly IndentedStringBuilder stringBuilder;

        public Indenter(IndentedStringBuilder stringBuilder)
        {
            this.stringBuilder = stringBuilder;

            this.stringBuilder.IncrementIndent();
        }

        public void Dispose() => this.stringBuilder.DecrementIndent();
    }

    private sealed class CodeBlockIndenter : IDisposable
    {
        private readonly IndentedStringBuilder stringBuilder;

        public CodeBlockIndenter(IndentedStringBuilder stringBuilder)
        {
            this.stringBuilder = stringBuilder;

            this.stringBuilder.AppendLine("{");
            this.stringBuilder.IncrementIndent();
        }

        public void Dispose()
        {
            this.stringBuilder.DecrementIndent();
            this.stringBuilder.AppendLine("}");
        }
    }

    private sealed class IndentSuspender : IDisposable
    {
        private readonly IndentedStringBuilder stringBuilder;
        private readonly int indent;

        public IndentSuspender(IndentedStringBuilder stringBuilder)
        {
            this.stringBuilder = stringBuilder;
            this.indent = stringBuilder.indent;
            this.stringBuilder.indent = 0;
        }

        public void Dispose() => this.stringBuilder.indent = this.indent;
    }
}
